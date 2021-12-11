using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
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
using streamer.db.Database.Dto;
using streamer.db.Database.Helpers;
using streamer.Features.Helpers;
using streamer.Features.Partner;
using streamer.Helpers;

namespace streamer.Controllers
{
    [Authorize]
    [ODataRoutePrefix("StreamerPreferences")]
    [Route("api/[controller]")]
    public class StreamerPreferences : ODataController
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly IMediator _mediator;

        public StreamerPreferences(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IMediator mediator)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

      
        [HttpGet("{type}")]
        public IQueryable<StreamerPreferenceDm> Get(PreferenceType type)
        {
            var userId = User.GetLoggedUserId();
            return _dbContext.StreamerPreferences
                .Include(x => x.Preference)
                .Where(x => x.StreamerId == userId &&  x.Preference.Type == type).AsNoTracking();
        }

        [HttpPost("{type}")]
        public async Task<IActionResult> Update(PreferenceType type, [FromBody] Guid[] preferencesId)
        {
            var userId = User.GetLoggedUserId();
            var toDelete = _dbContext.StreamerPreferences
                .Include(x => x.Preference)
                .Where(x => x.StreamerId == userId && x.Preference.Type == type);
            _dbContext.StreamerPreferences.RemoveRange(toDelete);
            
            var newPreferences = preferencesId.Select(preferenceId => new StreamerPreferenceDm() {StreamerId = userId, PreferenceId = preferenceId});
            await _dbContext.StreamerPreferences.AddRangeAsync(newPreferences);
            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }

    }
}