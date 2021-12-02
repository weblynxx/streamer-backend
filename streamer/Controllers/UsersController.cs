using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using streamer.ApiModel.Auth;
using streamer.ApiModel.settings;
using streamer.db.Database.DataModel;
using streamer.db.Database.Helpers;
using streamer.Features.interfaces.services;
using streamer.Features.User;
using streamer.Features.User.Login;
using streamer.Helpers;
using streamer.Security;
using streamer.Services;

namespace streamer.Controllers
{
    [Authorize]
    [ODataRoutePrefix("Users")]
    [ApiController]
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly HttpClient Client = new HttpClient();
        private readonly UserManager<StreamerDm> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly IUserService _userService;
        private readonly SignInManager<StreamerDm> _signInManager;
        private readonly AuthSettings authSettings;
        private readonly AppSettings _appSettings;
        private readonly IMediator _mediator;

        public UsersController(UserManager<StreamerDm> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, IUserService userService,
            SignInManager<StreamerDm> signInManager,
            IMediator mediator, IOptions<AuthSettings> authSettings, IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.jwtFactory = jwtFactory;
            this.jwtOptions = jwtOptions.Value;
            _userService = userService;
            _signInManager = signInManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.authSettings = authSettings.Value ?? throw new ArgumentNullException(nameof(authSettings));
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult Values ()
        {
            return Ok("Test");
        }

        [HttpGet("{id}/password")]
        public IActionResult AccountWithPassword(Guid id)
        {
            var user = _userService.AccountWithPassword(id);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            Logger.Debug(JsonConvert.SerializeObject(user));
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateStreamer([FromBody] StreamerDm userParam)
        {
            var password = userParam.Password;
            Regex regex = new Regex("^(?=.*[a-z,ä,ö,ü])(?=.*[A-Z,Ä,Ö,Ü])(?=.*[0-9])(?=.*[!@_#?.$%^&*/\\\\])(?=.{8,})");
            var isPasswordStrong = regex.IsMatch(password);
            if (!isPasswordStrong)
            {
                return BadRequest("password_strong");
            }

            userParam.Token = "";
            userParam.Password = "";
            userParam.CreatedDate = DateTime.UtcNow;
            userParam.StreamerId = Guid.NewGuid();
            try
            {
                var result = await userManager.CreateAsync(userParam, password);
                if (result.Succeeded)
                {
                    Logger.Debug($"userParam: {JsonConvert.SerializeObject(userParam)}");
                    Logger.Debug("Ok");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Error("BadRequest");
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePartner([FromBody] StreamerDm userParam)
        {
            var password = userParam.Password;
            Regex regex = new Regex("^(?=.*[a-z,ä,ö,ü])(?=.*[A-Z,Ä,Ö,Ü])(?=.*[0-9])(?=.*[!@_#?.$%^&*/\\\\])(?=.{8,})");
            var isPasswordStrong = regex.IsMatch(password);
            if (!isPasswordStrong)
            {
                return BadRequest("password_strong");
            }

            userParam.Token = "";
            userParam.Password = "";
            userParam.Authorities = "ROLE_PARTNER";
            userParam.CreatedDate = DateTime.UtcNow;
            try
            {
                var result = await userManager.CreateAsync(userParam, password);
                if (result.Succeeded)
                {
                    Logger.Debug($"userParam: {JsonConvert.SerializeObject(userParam)}");
                    Logger.Debug("Ok");
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Error("BadRequest");
            return BadRequest();
        }

        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> login([FromBody] LoginApiModel credentials, [FromServices] LoginCommand command)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            command.UserName = credentials.UserName;
            command.Password = credentials.Password;
            command.RememberMe = credentials.RememberMe ?? false;
            command.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var response = await _mediator.Send(command);
            if (response.Errors.Any())
            {
                Logger.Error(JsonConvert.SerializeObject(response));
                var xx = response.Errors.FirstOrDefault().Description;
                return BadRequest(xx);
            }

            Logger.Debug(JsonConvert.SerializeObject(response));
            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            Logger.Debug("OkObjectResult");
            return new OkObjectResult("login");
        }
        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.Claims.Single(c => c.Type == "id");
            var user = await userManager.FindByIdAsync(userId.Value);
            var model = await _mediator.Send(new AccountById.Query { Id = user.Id });
            Logger.Debug(JsonConvert.SerializeObject(user));
            return new OkObjectResult(new { account = user });
        }

        [HttpGet("info")]
        [AllowAnonymous]
        public IActionResult info()
        {
            var mode = string.IsNullOrEmpty(_appSettings.Mode) ? "" : $" | Mode={_appSettings.Mode}";

            // version based on GitVersion
            var version = new { SemVer = "", CommitDate = "", InformationalVersion = "" };
            var commit = new { Sha = "", ShortSha = "" };
            var gitVersionInformationType = typeof(UsersController).Assembly.GetType("GitVersionInformation");
            if (gitVersionInformationType != null)
            {
                commit = new
                {
                    Sha = $"{gitVersionInformationType.GetField("Sha").GetValue(null)}",
                    ShortSha = _appSettings.Mode.ToLower() == "production" ? "20.10.2021" : $"{gitVersionInformationType.GetField("ShortSha").GetValue(null)}"
                };
                version = new
                {
                    SemVer = _appSettings.Mode.ToLower() == "production" ? "2021.42.1667" : $"{gitVersionInformationType.GetField("SemVer").GetValue(null)}{mode}",
                    CommitDate = $"{gitVersionInformationType.GetField("CommitDate").GetValue(null)}",
                    InformationalVersion = $"{gitVersionInformationType.GetField("InformationalVersion").GetValue(null)}"
                };
                // if need other version property https://gitversion.net/docs/usage/msbuild-task
            }

            return new OkObjectResult(new { version, commit, mode });
        }


    }
}
