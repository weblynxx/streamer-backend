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
    [ODataRoutePrefix("StreamerPartners")]
    [Route("api/[controller]")]
    public class StreamerPartners : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly IMediator _mediator;

        public StreamerPartners(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IMediator mediator)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

      
        [HttpGet("{type}")]
        public IQueryable<StreamerPartnereDm> Get(DeliveryType type)
        {
            var userId = User.GetLoggedUserId();
            return _dbContext.StreamerPartners
                .Include(x => x.Partner)
                .Where(x => x.StreamerId == userId &&  x.Partner.Type == type).AsNoTracking();
        }

        [HttpPost("{type}")]
        public async Task<IActionResult> Update(DeliveryType type, [FromBody] Guid[] partnersId)
        {
            var userId = User.GetLoggedUserId();
            var toDelete = _dbContext.StreamerPartners
                .Include(x => x.Partner)
                .Where(x => x.StreamerId == userId && x.Partner.Type == type);
            _dbContext.StreamerPartners.RemoveRange(toDelete);
            
            var newPartners = partnersId.Select(partnerId => new StreamerPartnereDm() {StreamerId = userId, PartnerId = partnerId});
            await _dbContext.StreamerPartners.AddRangeAsync(newPartners);
            await _dbContext.SaveChangesAsync();
            
            return Ok();
        }

    }
}