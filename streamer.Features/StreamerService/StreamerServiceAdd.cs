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
    public class StreamerServiceAdd
    {
        public class StreamerServiceAddCommand : StreamerServiceDm, IRequest<Response>
        {
        }

        public class Validator : AbstractValidator<StreamerServiceAddCommand>
        {
            public Validator()
            {
            }
        }

        public class StreamerServiceAddUpdatHandler : IRequestHandler<StreamerServiceAddCommand, Response>
        {
            private readonly StreamerDbContext _dbContext;
            private readonly IMapper _mapper;

            public StreamerServiceAddUpdatHandler(StreamerDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<Response> Handle(StreamerServiceAddCommand request, CancellationToken cancellationToken)
            {
                var newEntry = new StreamerServiceDm();
                _mapper.Map(request, newEntry);
                _dbContext.StreamerServices.Add(newEntry);
                await _dbContext.SaveChangesAsync();

                return new Response { Id = newEntry.Id, IsValid = true };
            }

            

        }
    }
}
