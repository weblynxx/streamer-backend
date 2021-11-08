using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace streamer.db.Database.Helpers
{
    public interface IUserResolveService
    {
        Task<string> GetCurrentSessionUserId(IdentityDbContext dbContext);
        string GetCurrentSessionUserName(StreamerDbContext dbContext);
        Guid GetCurrentSessionCompanyId(StreamerDbContext dbContext);
    }
}
