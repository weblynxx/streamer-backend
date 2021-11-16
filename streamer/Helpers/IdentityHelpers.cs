using System;
using System.Security.Claims;
using streamer.Security;

namespace streamer.Helpers
{
    /// <summary>
    /// IdentityExtensions
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Get UserId
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Guid GetLoggedUserId(this ClaimsPrincipal principal)
        {
            var claim = principal?.FindFirst(JwtConstants.IdClaim);

            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }


    }
}
