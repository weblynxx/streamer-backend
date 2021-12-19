using System;
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
    [ODataRoutePrefix("Preferences")]
    [Route("api/[controller]")]
    public class PreferencesController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly StreamerDbContext _dbContext;
        private readonly IOptions<AppSettings> _config;
        private readonly UserManager<StreamerDm> _userManager;
        private readonly IMediator _mediator;

        public PreferencesController(StreamerDbContext dbContext, IOptions<AppSettings> config,
            UserManager<StreamerDm> userManager, IMediator mediator)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

      
        [HttpGet("[action]/{type}")]
        public IQueryable<PreferenceDm> Get(PreferenceType type)
        {
            return _dbContext.Preferences.Where(x => x.Type == type).AsNoTracking();
        }

    }
}