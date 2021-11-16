using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using streamer.db.Database.DataModel;
using streamer.db.Database.DataModelConfigurations;
using streamer.db.Database.Extension;
using streamer.db.Database.Helpers;

namespace streamer.db
{
    public class StreamerDbContext : IdentityDbContext<StreamerDm, IdentityRole<Guid>, Guid>
    {
        private readonly IConfiguration _configuration;
        private IDbContextTransaction _currentTransaction;
        private IUserResolveService _userResolveService;

        public DbSet<StreamerDm> Streamers { get; set; }
        public DbSet<ServiceDm> Services { get; set; }
        public DbSet<StreamerServiceDm> StreamerServices { get; set; }
        public DbSet<RefreshTokenDm> RefreshTokens { get; set; }

        public StreamerDbContext(IUserResolveService userResolveService, DbContextOptions<StreamerDbContext> options, IConfiguration configuration) : base(options)
        {
            _userResolveService = userResolveService;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //var appSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
                var dbUsername = "duda";
                var dbPassword = "duda";

                optionsBuilder.UseNpgsql($"Host=localhost;Database=streamer;Username={dbUsername};Password={dbPassword}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
            modelBuilder.ApplyConfiguration(new StreamerConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new StreamerServiceConfiguration());
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }

}
