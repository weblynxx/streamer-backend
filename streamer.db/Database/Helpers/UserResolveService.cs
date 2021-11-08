using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace streamer.db.Database.Helpers
{
    public class UserResolveService : IUserResolveService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserResolveService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetCurrentSessionUserId(IdentityDbContext dbContext)
        {
            var currentSessionUserEmail = _httpContextAccessor.HttpContext.User.Identity.Name;

            var user = await dbContext.Users
                .SingleAsync(u => u.Email.Equals(currentSessionUserEmail));
            return user.Id;
        }

        public string GetCurrentSessionUserName(StreamerDbContext dbContext)
        {
            const string jwtConstantsIdClaim = "id";
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(jwtConstantsIdClaim);
            return userId;
        }

        public Guid GetCurrentSessionCompanyId(StreamerDbContext dbContext)
        {
            const string jwtConstantsCidClaim = "cid";
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(jwtConstantsCidClaim);
            return Guid.Parse(userId);
        }
    }
}
