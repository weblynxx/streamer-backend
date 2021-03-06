using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;

        public StreamersController(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
            _env = env;
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
        [HttpGet("[action]/{username}")]
        public async Task<IActionResult> GetStreamerInfoByUserName(string username)
        {
            var streamer = _dbContext.Streamers
                .AsNoTracking()
                .Include(x => x.StreamerServices).ThenInclude(x => x.Service)
                .Include(x => x.StreamerPartners).ThenInclude(x => x.Partner)
                .Include(x => x.StreamerPreferences).ThenInclude(x => x.Preference)
                .FirstOrDefault(p => p.UserName.ToLower() == username.ToLower());
            
            if (streamer == null)
            {
                return BadRequest("streamer not found");
            }

            var result = new StreamerDto()
            {
                FirstName = streamer.FirstName,
                LastName = streamer.LastName,
                UserName = streamer.UserName,
                isStoppedDelivery = streamer.isStoppedDelivery,
                From = streamer.From,
                To = streamer.To,
                StreamerId = streamer.StreamerId,
                Services = streamer
                    .StreamerServices
                    .Select(
                        x => new StreamerServiceDto()
                        {
                            Id = x.Id, ServiceId = x.ServiceId, ServiceName = x.Service.Name, ServiceUserName = x.ServiceUserName
                        }).ToList(),
                PartnersFood = streamer.StreamerPartners
                    .Where(x =>x.Partner.Type == DeliveryType.Food)
                    .Select(x => new StreamerPartnerDto()
                    {
                        Id = x.Id, DeliveryName = x.Partner.DeliveryName, 
                        Logo = GetLogoByPartnerId(x.PartnerId)
                    }).ToList(),
                PartnersClothes = streamer.StreamerPartners
                    .Where(x => x.Partner.Type == DeliveryType.Clothes)
                    .Select(x => new StreamerPartnerDto()
                    {
                        Id = x.Id, DeliveryName = x.Partner.DeliveryName,
                        Logo = GetLogoByPartnerId(x.PartnerId)
                    }).ToList(),
                FoodPreferenceText = streamer.FoodPreferenceText,
                ClothesPreferenceText = streamer.ClothesPreferenceText,
                PreferencesFood = streamer.StreamerPreferences.Where(x => x.Preference.Type == PreferenceType.Food).Select(x => x.Preference).ToList(),
                PreferencesClothes= streamer.StreamerPreferences.Where(x => x.Preference.Type == PreferenceType.Clothes).Select(x => x.Preference).ToList()

            };
            
            return Ok(result);

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

        [HttpGet("[action]")]
        public string GetLogo()
        {
            var result = "";
            var userId = User.GetLoggedUserId();
            if (userId == Guid.Empty)
                return result;
            var mainImage = Path.Combine(_env.WebRootPath, userId.ToString(), "logo.png");

            Logger.Debug(mainImage);
            if (System.IO.File.Exists(mainImage))
            {
                result = FromFileToBase64String(mainImage);

            }
            else
            {
                Logger.Debug($"File {mainImage} not exist");
            }

            return result;
        }

        [AllowAnonymous]
        [HttpGet("[action]/{userName}")]
        public string GetLogoByUserName(string userName)
        {
            var result = "";
            if (userName == "")
            {
                return result;
            }

            var userId = _dbContext.Streamers.SingleOrDefault(x => x.UserName == userName).Id;
            if (userId == Guid.Empty)
                return result;
            var mainImage = Path.Combine(_env.WebRootPath, userId.ToString(), "logo.png");

            Logger.Debug(mainImage);
            if (System.IO.File.Exists(mainImage))
            {
                result = FromFileToBase64String(mainImage);

            }
            else
            {
                Logger.Debug($"File {mainImage} not exist");
            }

            return result;
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

        [HttpPost("[action]")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadLogo(ImageFile imagefile)
        {

            if (imagefile == null || imagefile.File.Length == 0)
                return Content("file not selected");
            var streamerId = User.GetLoggedUserId();

            try
            {
                var pathToSave = Path.Combine(_env.WebRootPath, streamerId.ToString());
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

        private async Task SendLinkToSetNewPasswordViaEmail(string email, string userName)
        {
            var streamer = String.IsNullOrEmpty(userName) ? _dbContext.Streamers.SingleOrDefault(x => x.Email.ToLower() == email.ToLower()): _dbContext.Streamers.SingleOrDefault(x => x.Email.ToLower() == email.ToLower() && x.UserName.ToLower() == userName.ToLower());
            if (streamer == null) return;
            var userToken = await _userManager.GenerateUserTokenAsync(streamer, "Email", "set-new-password");
            //await SendNewPasswordViaEmail(streamer.UserName, $"{userToken}/user/{streamer.Id}", streamer.Email);
            Logger.Debug($"Sended link to set new password for email={email}");
        }

        private string GetLogoByPartnerId(Guid partnerId)
        {
            var result = "";
            if (partnerId == Guid.Empty)
                return result;
            var mainImage = Path.Combine(_env.WebRootPath, partnerId.ToString(), $"{partnerId}.png");

            Logger.Debug(mainImage);
            if (System.IO.File.Exists(mainImage))
            {
                result = FromFileToBase64String(mainImage);

            }
            else
            {
                Logger.Debug($"File {mainImage} not exist");
            }

            return result;
        }
        private string FromFileToBase64String(string file)
        {
            try
            {
                using (Image image = Image.FromFile(file))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        var base64String = Convert.ToBase64String(imageBytes);
                        return "data:image/png;base64," + base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return ex.ToString();
            }
        }

    }
}