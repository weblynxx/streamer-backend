using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using streamer.db;
using streamer.db.Database.DataModel;
using streamer.Features.Behaviors;

namespace streamer.Features.Partner
{
    public class PartnerAddUpdate
    {
        public class PartnerAddUpdateCommand : PartnerDm, IRequest<Response>
        {
        }

        public class Validator : AbstractValidator<PartnerAddUpdateCommand>
        {
            public Validator()
            {
            }
        }

        public class PartnerAddUpdateHandler : IRequestHandler<PartnerAddUpdateCommand, Response>
        {
            private readonly StreamerDbContext _dbContext;
            private readonly IMapper _mapper;

            public PartnerAddUpdateHandler(StreamerDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<Response> Handle(PartnerAddUpdateCommand request, CancellationToken cancellationToken)
            {

                return request.Id != Guid.Empty ? Edit(request) : Add(request);
            }

            protected async Task<Response> Add(PartnerAddUpdateCommand message)
            {
                var newEntry = new PartnerDm();
                message.Id = Guid.NewGuid();
                _mapper.Map(message, newEntry);

                _dbContext.Partners.Add(newEntry);
                await _dbContext.SaveChangesAsync();

                return new Response { Id = newEntry.Id, IsValid = true };
            }

            protected async Task<Response> Edit(PartnerAddUpdateCommand message)
            {
                var oldEntry = _dbContext.Partners.SingleOrDefault(x => x.Id == message.Id );

                oldEntry.DeliveryName = message.DeliveryName;
                
                _dbContext.Partners.Update(oldEntry);
                await _dbContext.SaveChangesAsync();

                return new Response { Id = oldEntry.Id, IsValid = true };
            }

        }
    }
}
