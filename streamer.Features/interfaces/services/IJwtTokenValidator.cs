using System.Security.Claims;

namespace streamer.Features.interfaces.services
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
