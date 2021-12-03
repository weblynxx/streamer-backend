using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using streamer.db;
using streamer.db.Database.DataModel;

namespace streamer.Features.Partner
{
    public class PartnerQueryById : IRequest<PartnerDm>
    {
        public Guid? Id { get; set; }
    }

    public class PartnerQueryByIdHandler : IRequestHandler<PartnerQueryById, PartnerDm>
    {
        private readonly StreamerDbContext _dbContext;

        public PartnerQueryByIdHandler(StreamerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<PartnerDm> Handle(PartnerQueryById request, CancellationToken cancellationToken)
        {
            var entity = _dbContext.Partners.SingleOrDefault(x =>  x.Id== request.Id);
            return Task.FromResult(entity);
        }
    }
}
