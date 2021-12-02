using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using streamer.db.Database.DataModel;

namespace streamer.db.Database.Extension
{
    public static class ModelBuilderExtensions
    {
        private static readonly PasswordHasher<StreamerDm> Hasher = new PasswordHasher<StreamerDm>();

        public static void Seed(this ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<ServiceDm>().HasData(
                new ServiceDm
                {
                    Id = new Guid("bfab82b1-7cc6-4d37-8686-83da4b90e995"),
                    Name = "Twitch"
                },
                new ServiceDm
                {
                    Id = new Guid("83655a7a-92e0-4ce0-9d0c-f36fd5692a2a"),
                    Name = "Youtube"
                }
           );

           modelBuilder.Entity<StreamerDm>().HasData(
               new StreamerDm
               {
                   Id = new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                   SecurityStamp = "09355be4-a18d-41a9-acce-dd48c68211d7",
                   UserName = "admin",
                   NormalizedUserName = "admin".ToUpper(),
                   FirstName = "John",
                   LastName = "Admin",
                   PasswordHash = Hasher.HashPassword(null, "fuckThisSheet_DF$%123!@#"),
                   Authorities = "ROLE_ADMIN",
               });

        }
    }
}
