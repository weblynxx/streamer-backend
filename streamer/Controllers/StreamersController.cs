using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog.Fluent;
using PasswordGenerator;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.db.Database.Helpers;
using streamer.Features.Helpers;
using streamer.Helpers;

namespace streamer.Controllers
{
    [Authorize]
    [ODataRoutePrefix("Streamers")]
    [Route("api/[controller]")]
    public class StreamersController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;

        public StreamersController(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("ValidateUserName")]
        public async Task<IActionResult> ValidateUserName([FromBody] StreamerDm streamer)
        {
            var isExistStreamer = _dbContext.Streamers.FirstOrDefault(p => p.UserName == streamer.UserName);
            if (isExistStreamer != null && isExistStreamer.Id != streamer.Id)
            {
                return BadRequest("already exist");
            }
            Logger.Debug().Message("Validated").Message($"WHERE UserName={streamer.UserName}").Write();
            return Ok();
        }
             
        [HttpPost("ValidateExistingEmail")]
        public async Task<IActionResult> ValidateExistingEmail([FromBody] StreamerDm streamer)
        {
            var isExistStreamerWithEmail = _dbContext.Streamers.FirstOrDefault(p => p.Email == streamer.Email);
            if (isExistStreamerWithEmail != null && isExistStreamerWithEmail.Id != streamer.Id)
            {
                Logger.Debug($"Alredy exist user with email={streamer.Email}, where username=\"{isExistStreamerWithEmail.UserName}\"");
                return BadRequest("user with same email already exist");
            }
            Logger.Debug($"WHERE email={streamer.Email}");
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GenerateNewStreamerId()
        {
            var userId = User.GetLoggedUserId();
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);
            oldStreamer.StreamerId = Guid.NewGuid();
            _dbContext.Streamers.Update(oldStreamer);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] StreamerDm streamer)
        {
            
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == streamer.Id);

            if (oldStreamer != null)
            {
                oldStreamer.FirstName = streamer.FirstName;
                oldStreamer.LastName = streamer.LastName;
                oldStreamer.Phone = streamer.Phone;
                oldStreamer.Authorities = streamer.Authorities;
                oldStreamer.Email = streamer.Email;
                oldStreamer.NormalizedEmail = streamer.Email.ToUpper();
                oldStreamer.UserName = streamer.UserName;
                oldStreamer.NormalizedUserName = streamer.UserName.ToUpper();
                if (!string.IsNullOrEmpty(streamer.Password))
                {
                    var newPasswordHash = _userManager.PasswordHasher.HashPassword(oldStreamer, streamer.Password);
                    oldStreamer.PasswordHash = newPasswordHash;
                }
                else
                {
                    _dbContext.Entry(oldStreamer).Property(x => x.Password).IsModified = false;    
                }
                _dbContext.Streamers.Update(oldStreamer);
                await _dbContext.SaveChangesAsync();
            }
            Logger.Debug().Message("Updated").Write();
            return Ok(streamer);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var toDelete = _dbContext.Streamers.SingleOrDefault(p => p.Id == id);
            _dbContext.Streamers.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
            Logger.Debug("Deleted");
            return Ok();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> GenerateNewPassword(Guid id)
        {
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == id);
            if (oldStreamer != null)
            {
                await SendLinkToSetNewPasswordViaEmail(oldStreamer.Email, oldStreamer.UserName);
            }
            Logger.Debug().Message("Returned").Message($"WHERE Id={id}").Write();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]/{email}")]
        public async Task<IActionResult> IsUniqueEmail(string email)
        {
            var emps = _dbContext.Streamers.Count(x => x.Email.ToLower() == email.ToLower());
            Logger.Debug($"IsUniqueEmail: For {email} there is/are {emps} username(s)");
            return Ok(emps);
        }

        [HttpPost("returnNewPassword")]
        public async Task<IActionResult> ReturnNewPassword(int id)
        {
            var pwd = new Password(includeLowercase: true, includeUppercase: true, includeNumeric: true,
                    includeSpecial: true, passwordLength: 8);
            
            var newPassword = pwd.Next();
            newPassword = newPassword.Replace('O', 'A').Replace('0', 'A').Replace('l', 'z').Replace('I', 'Z');
            Logger.Debug().Message("Returned").Write();
            return Ok(newPassword);
        }
        
        private async Task SendLinkToSetNewPasswordViaEmail(string email, string userName)
        {
            var streamer = String.IsNullOrEmpty(userName) ? _dbContext.Streamers.SingleOrDefault(x => x.Email.ToLower() == email.ToLower()): _dbContext.Streamers.SingleOrDefault(x => x.Email.ToLower() == email.ToLower() && x.UserName.ToLower() == userName.ToLower());
            if (streamer == null) return;
            var userToken = await _userManager.GenerateUserTokenAsync(streamer, "Email", "set-new-password");
            //await SendNewPasswordViaEmail(streamer.UserName, $"{userToken}/user/{streamer.Id}", streamer.Email);
            Logger.Debug($"Sended link to set new password for email={email}");
        }

    }
}