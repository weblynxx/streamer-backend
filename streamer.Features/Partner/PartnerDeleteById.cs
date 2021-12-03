using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using streamer.db;
using streamer.Features.Behaviors;

namespace streamer.Features.Partner
{

    public class PartnerDeleteByIdCommand : IRequest<Response>
    {
        public Guid Id { get; set; }
    }

    public class PartnerDeleteByIdHandler : IRequestHandler<PartnerDeleteByIdCommand, Response>
    {
        private readonly StreamerDbContext _dbContext;

        public PartnerDeleteByIdHandler(StreamerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(PartnerDeleteByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _dbContext.Partners.SingleOrDefault(p => p.Id == request.Id );
                if (entity != null)
                    _dbContext.Partners.Remove(entity);

                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                var response = new Response(entity) { Id = entity.Id };
                return response;
            }
            catch (Exception e)
            {
                return Response.AsErrorResult(e.Message);
            }
        }
    }
}
