using System;
using System.Linq;
using System.Security.Claims;
using streamer.Security;

namespace streamer.Helpers
{
    public static class ClaimsEx
    {
        public static Guid? GetCompanyId(this ClaimsPrincipal user)
        {
            var value = user.Claims.FirstOrDefault(x => x.Type == "cid")?.Value;
            if (string.IsNullOrEmpty(value)) return null;
            return Guid.Parse(value);
        }
        public static Guid GetId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.Single(c => c.Type == "id").Value);
        }
        public static Guid GetTypeId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.Claims.Single(c => c.Type == JwtConstants.IdTypeClaim).Value);
        }
    }

}
