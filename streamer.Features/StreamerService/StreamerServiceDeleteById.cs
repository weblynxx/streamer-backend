using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using streamer.db;
using streamer.Features.Behaviors;

namespace streamer.Features.StreamerService
{

    public class StreamerServiceDeleteByIdCommand : IRequest<Response>
    {
        public Guid StreamerId { get; set; }
        public Guid ServiceId { get; set; }
    }

    public class EmployeeProjectDeleteByIdHandler : IRequestHandler<StreamerServiceDeleteByIdCommand, Response>
    {
        private readonly StreamerDbContext _dbContext;

        public EmployeeProjectDeleteByIdHandler(StreamerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Response> Handle(StreamerServiceDeleteByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _dbContext.StreamerServices.SingleOrDefault(p => p.ServiceId == request.ServiceId && p.StreamerId== request.StreamerId);
                if (entity != null)
                    _dbContext.StreamerServices.Remove(entity);

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
