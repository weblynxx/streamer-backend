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
        /// Get CompanyId
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Guid GetLoggedUserCompanyId(this ClaimsPrincipal principal)
        {
            var claim = principal?.FindFirst(JwtConstants.IdCompanyClaim);

            return claim == null ? Guid.Empty : Guid.Parse(claim.Value);
        }


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


        public static bool IsImpersonating(this ClaimsPrincipal principal)
        {
            var claim = principal?.FindFirst(JwtConstants.IsImpersonating);

            return claim == null ? false : true;
        }


    }
}
