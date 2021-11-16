using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.Features.Behaviors;

namespace streamer.Features.StreamerService
{
    public class StreamerServiceUpdate
    {
        public class StreamerServiceUpdateCommand : StreamerServiceDm, IRequest<Response>
        {
        }

        public class Validator : AbstractValidator<StreamerServiceUpdateCommand>
        {
            public Validator()
            {
            }
        }

        public class StreamerServiceUpdateHandler : IRequestHandler<StreamerServiceUpdateCommand, Response>
        {
            private readonly StreamerDbContext _dbContext;
            private readonly IMapper _mapper;

            public StreamerServiceUpdateHandler(StreamerDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<Response> Handle(StreamerServiceUpdateCommand request, CancellationToken cancellationToken)
            {
                var oldEntry = _dbContext.StreamerServices
                    .SingleOrDefault(
                        x => 
                            x.StreamerId == request.StreamerId
                            && x.ServiceId == request.ServiceId);

                _mapper.Map(request, oldEntry);
                _dbContext.StreamerServices.Update(oldEntry);
                _dbContext.SaveChangesAsync();

                return Task.FromResult(new Response { Id = oldEntry.Id, IsValid = true });
            }

            

        }
    }
}
