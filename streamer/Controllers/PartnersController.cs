using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Fluent;
using PasswordGenerator;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.db.Database.Dto;
using streamer.db.Database.Helpers;
using streamer.Features.Helpers;
using streamer.Features.Partner;
using streamer.Helpers;

namespace streamer.Controllers
{
    public class ImageFile
    {
        public IFormFile File { get; set; }
        public string TargetFolder { get; set; }
    }

    [Authorize]
    [ODataRoutePrefix("Partners")]
    [Route("api/[controller]")]
    public class PartnersController : ODataController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly IMediator _mediator;
        private readonly IHostingEnvironment _env;

        public PartnersController(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IMediator mediator, IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _config = config;
            _env = env;
            _userManager = userManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        [HttpPost("[action]/{deliveryType}")]
        public async Task<IActionResult> CreatePartner([FromBody] StreamerDm userParam, DeliveryType type)
        {
            var userId = User.GetLoggedUserId();
            var user = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);
            if (user == null || user.Authorities != "ROLE_ADMIN")
            {
                return BadRequest("require_role_admin_privilege");
            }

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
                var result = await _userManager.CreateAsync(userParam, password);
                if (result.Succeeded)
                {
                    var createdAppUser = await _userManager.FindByEmailAsync(userParam.Email);
                    var model = await _mediator.Send(new PartnerAddUpdate.PartnerAddUpdateCommand(){ Streamer = createdAppUser, DeliveryName = createdAppUser.FirstName, Type = type});
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

        [ODataRoute]
        [HttpGet("[action]")]
        [EnableQuery(PageSize = 25, AllowedQueryOptions = AllowedQueryOptions.All
            , HandleNullPropagation = HandleNullPropagationOption.True)]
        public IQueryable<PartnerDto> Get()
        {
            return _dbContext.Partners.AsNoTracking().Include(x => x.Streamer).Select(x => new PartnerDto()
            {
                Id = x.Id,
                FirstName = x.Streamer.FirstName,
                LastName =  x.Streamer.LastName,
                Email = x.Streamer.Email,
                UserName = x.Streamer.UserName,
                DeliveryName = x.DeliveryName,
                DeliveryType = x.Type
        }).AsQueryable();
        }

        [HttpPost("[action]")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImage(ImageFile imagefile)
        {

            if (imagefile == null || imagefile.File.Length == 0)
                return Content("file not selected");

            try
            {
                var pathToSave = Path.Combine(_env.WebRootPath, imagefile.TargetFolder);
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                Logger.Debug("Check image size");

                if (imagefile.File.Length > 0)
                {
                    Logger.Debug($"Image size = {imagefile.File.Length}");
                    var fileName = $"{imagefile.TargetFolder}.png";//ContentDispositionHeaderValue.Parse(imagefile.File.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await imagefile.File.CopyToAsync(stream);
                    }

                    Logger.Debug("Image uploaded");
                    return Ok(new { fullPath });
                }
                else
                {
                    Logger.Debug("Image not uploaded");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return StatusCode(500, "Internal server error " + ex);
            }
        }

    }
}