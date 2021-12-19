using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost("IsExistStreamerByUserName")]
        public async Task<bool> IsExistStreamerByUserName([FromBody] StreamerDm streamer)
        {
            var isExistStreamer = _dbContext.Streamers
                .AsNoTracking()
                .FirstOrDefault(p => p.UserName.ToLower() == streamer.UserName.ToLower());
            if (isExistStreamer != null)
            {
                return true;
            }
            return false;
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

        /// <summary>
        /// Return address info of Streamer by StreamerId
        /// Requires only PARTER_ROLE
        /// </summary>
        /// <param name="streamerId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{streamerId}")]
        public async Task<IActionResult> GetStreamerAddressInfo(Guid streamerId)
        {
            var partnerId = User.GetLoggedUserId();
            var partner = _dbContext.Streamers.SingleOrDefault(x => x.Id == partnerId);
            if (partner == null || partner.Authorities != "ROLE_PARTNER")
            {
                return BadRequest("invalid_privileges");
            }

            var streamer = _dbContext.Streamers.SingleOrDefault(x => x.StreamerId == streamerId);
            if (streamer == null)
            {
                return BadRequest($"Can't find Streamer with streamerId - {streamerId}");
            }
            
            return Ok(new
            {
                FirstName = streamer.FirstName,
                LastName = streamer.LastName,
                Phone = streamer.Phone,
                City = streamer.City,
                Street = streamer.Street,
                House = streamer.House,
                HouseBuilding = streamer.HouseBuilding,
                Entrance = streamer.Entrance,
                Floor = streamer.Floor,
                Flat = streamer.Flat,
                IntercomCode = streamer.IntercomCode
            });
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

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateContactData([FromBody] StreamerDm streamer)
        {
            var userId = User.GetLoggedUserId();
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);

            if (oldStreamer != null)
            {
                oldStreamer.City = streamer.City;
                oldStreamer.Phone = streamer.Phone;
                oldStreamer.Street = streamer.Street;
                oldStreamer.House = streamer.House;
                oldStreamer.HouseBuilding = streamer.HouseBuilding;
                oldStreamer.Entrance = streamer.Entrance;
                oldStreamer.Floor = streamer.Floor;
                oldStreamer.Flat = streamer.Flat;
                oldStreamer.IntercomCode = streamer.IntercomCode;
               
                _dbContext.Streamers.Update(oldStreamer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("user not found");
            }

            Logger.Debug("Updated");
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateTimeDelivery([FromBody] StreamerDm streamer)
        {
            var userId = User.GetLoggedUserId();
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);

            if (oldStreamer != null)
            {
                oldStreamer.From = streamer.From;
                oldStreamer.To = streamer.To;
                oldStreamer.isStoppedDelivery = streamer.isStoppedDelivery;

                _dbContext.Streamers.Update(oldStreamer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("user not found");
            }

            Logger.Debug("Updated");
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateFoodPreferenceText([FromBody] StreamerDm streamer)
        {
            var userId = User.GetLoggedUserId();
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);

            if (oldStreamer != null)
            {
                oldStreamer.FoodPreferenceText= streamer.FoodPreferenceText;

                _dbContext.Streamers.Update(oldStreamer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("user not found");
            }

            Logger.Debug("Updated");
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateClothesPreferenceText([FromBody] StreamerDm streamer)
        {
            var userId = User.GetLoggedUserId();
            var oldStreamer = _dbContext.Streamers.SingleOrDefault(x => x.Id == userId);

            if (oldStreamer != null)
            {
                oldStreamer.ClothesPreferenceText= streamer.ClothesPreferenceText;

                _dbContext.Streamers.Update(oldStreamer);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("user not found");
            }

            Logger.Debug("Updated");
            return Ok();
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