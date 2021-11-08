using System;
using System.Security.Claims;
using System.Threading.Tasks;
using streamer.Features.User;

namespace streamer.Features.interfaces.services
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(string userName, Guid id);
    }
}
