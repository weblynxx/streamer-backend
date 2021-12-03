﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
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
using streamer.Features.Partner;
using streamer.Helpers;

namespace streamer.Controllers
{
    [Authorize]
    [ODataRoutePrefix("Partners")]
    [Route("api/[controller]")]
    public class PartnersController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly IMediator _mediator;

        public PartnersController(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IMediator mediator)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        [HttpPost("[action]/{deliveryName}/{deliveryType}/{deliveryType}")]
        public async Task<IActionResult> CreatePartner([FromBody] StreamerDm userParam, string deliveryName, DeliveryType type)
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
                    var model = await _mediator.Send(new PartnerAddUpdate.PartnerAddUpdateCommand(){ Streamer = createdAppUser, DeliveryName = deliveryName, Type = type});

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Logger.Error("BadRequest");
            return BadRequest();
        }

        [HttpGet("[action]")]
        public IQueryable Get()
        {
            return _dbContext.Partners.Include(x => x.Streamer).Select(x => new
            {
                Id = x.Id,
                FirstName = x.Streamer.FirstName,
                LastName =  x.Streamer.LastName,
                email = x.Streamer.Email,
                UserName = x.Streamer.UserName,
                DeliveryName = x.DeliveryName
        });
        }

    }
}