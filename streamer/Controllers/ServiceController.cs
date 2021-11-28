using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using streamer.ApiModel.Auth;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.db.Database.Helpers;
using streamer.Features.StreamerService;
using streamer.Helpers;

namespace streamer.Controllers
{
    [Authorize]
    [ODataRoutePrefix("Services")]
    [Route("api/[controller]")]
    public class ServiceController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly AppSettings _config;
        private readonly UserManager<StreamerDm> _userManager;
        private static readonly HttpClient Client = new HttpClient();
        private readonly IMediator _mediator;

        public ServiceController(StreamerDbContext dbContext, IOptions<AppSettings> config, IMediator mediator)
        {
            _dbContext = dbContext;
            _config = config.Value;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("[action]")]
        public IQueryable<ServiceDm> Get()
        {
            return _dbContext.Services.AsNoTracking();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> TwitchLogin([FromBody] TwitchAuthViewModel model)
        {
            // Check if user already connect Twitch
            var twitchId = _dbContext.Services.SingleOrDefault(x => x.Name.ToLower() == "twitch").Id;
            if (IsCurrentStreamerLoggedInService(twitchId))
            {
                return BadRequest("user_already_added_service");
            }
            Logger.Debug("Ok. User didn't link twitch account");
            var clientId = _config.TwitchClientId;
            var clientSecret = _config.TwitchSecretId;
            var redirect = _config.Domain;
            var redirect_uri = redirect == "localhost" ? "http://localhost:2002" : $"https://{redirect}";
            Logger.Debug("Try to get access token.....");
            var appAccessTokenResponse = await Client.PostAsync(
                $"https://id.twitch.tv/oauth2/token?client_id={clientId}&client_secret={clientSecret}&code={model.AccessToken}&grant_type=authorization_code&redirect_uri={redirect_uri}", null
            );
            var accessToken = JsonConvert.DeserializeObject<TwitchUserAccessTokenData>(await appAccessTokenResponse.Content.ReadAsStringAsync()).AccessToken;
            var userName = GetTwitchUserName(accessToken).Result;
            if (userName == "")
            {
                return BadRequest("Error when get username for twitch account");
            }
            Logger.Debug($"Get twitch username - {userName}");

            var command = new StreamerServiceAdd.StreamerServiceAddCommand()
            {
                StreamerId = User.GetLoggedUserId(),
                ServiceId = twitchId,
                ServiceUserName = userName
            };
            var response = await _mediator.Send(command);
            if (response.IsValid)
            {
                return Ok();
            }
            return BadRequest(response.Errors);
        }

        private bool IsCurrentStreamerLoggedInService(Guid serviceId)
        {
            var userId = User.GetLoggedUserId();
            var isAlreadyExist = _dbContext.StreamerServices.FirstOrDefault(x => x.ServiceId == serviceId && x.StreamerId == userId);
            return isAlreadyExist != null;
        }

        private async Task<string> GetTwitchUserName(string accessToken)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request =
                        new HttpRequestMessage(new HttpMethod("GET"), "https://api.twitch.tv/helix/users"))
                    {
                        request.Headers.TryAddWithoutValidation("Client-Id", _config.TwitchClientId);
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                        var responseJson = await httpClient.SendAsync(request);
                        var contentString = await responseJson.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<TwitchUsersData>(contentString);
                        return res.Data.First().DisplayName;
                    }
                }
            }
            catch (Exception e)
            {
                return "";
            }
            
        }

    }
}
