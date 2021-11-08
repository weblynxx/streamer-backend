using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using streamer.db;

namespace streamer.Features.User
{
    public class AccountById
    {
        public class Model
        {
            public Guid? Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public bool InvolvableInProjects { get; set; }
        }

        public class Query : IRequest<Model>
        {
            public Guid? Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly ILogger<Handler> _logger;
            private readonly StreamerDbContext _streamerDbContext;

            public Handler(ILogger<Handler> logger, StreamerDbContext streamerDbContext)
            {
                _logger = logger;
                _streamerDbContext = streamerDbContext;
            }

            public Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = _streamerDbContext.Users.AsNoTracking()
                    .Where(x => x.Id == request.Id)
                    .Select(s => new Model
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                    })
                    .SingleOrDefault();
                return Task.FromResult(company);
            }

        }
    }
}
