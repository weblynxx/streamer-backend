using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using streamer.db;
using streamer.db.Database.DataModel;

namespace streamer.Features.Partner
{
    public class PartnerQueryAll : IRequest<IQueryable<PartnerDm>>
    {
    }

    public class PartnerQueryAllHandler : IRequestHandler<PartnerQueryAll, IQueryable<PartnerDm>>
    {
        private readonly StreamerDbContext _dbContext;

        public PartnerQueryAllHandler(StreamerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IQueryable<PartnerDm>> Handle(PartnerQueryAll request, CancellationToken cancellationToken)
        {
            var entityList = _dbContext.Partners.AsNoTracking(); 
            return Task.FromResult(entityList);
        }
    }
}
