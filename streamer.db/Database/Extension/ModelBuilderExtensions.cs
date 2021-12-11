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
           modelBuilder.Entity<PreferenceDm>().HasData(
               // FOOD
               new PreferenceDm()
               {
                   Id = new Guid("026be9b4-beaa-45aa-876d-271aa539a8fb"),
                   Name = "pizza",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("0be66cc3-6804-4885-8acf-1b49f46b1a58"),
                   Name = "sushi",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("0cecd9ee-f8b0-46fc-92a3-e2c847f531fc"),
                   Name = "steaks",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("242cf954-086c-4c0f-9930-b22591ce978e"),
                   Name = "hot_dogs",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("43b0e39a-7a4e-4798-9249-e56cb188518f"),
                   Name = "vietnamese_food",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("5f453949-9525-408c-b558-889bfb52632a"),
                   Name = "georgian_food",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("6a1b34e4-24ac-46ba-b597-6104485b4bfd"),
                   Name = "seafood",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("71beecfb-66e5-4432-904f-e0033ae2b9e7"),
                   Name = "bakery_products",
                   Type = PreferenceType.Food
               },
               new PreferenceDm()
               {
                   Id = new Guid("8bd2091c-78d2-4595-8679-ee5394357870"),
                   Name = "burgers",
                   Type = PreferenceType.Food
               },


               // CLOTHES
               new PreferenceDm()
               {
                   Id = new Guid("8d232f73-4430-4498-9b26-4490c083eb42"),
                   Name = "footwear",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("af250697-517c-4368-9e9c-aa6cba27eb33"),
                   Name = "head",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("b1920383-6d50-412f-860f-c5f255404930"),
                   Name = "trousers",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("b6f9de67-64c2-4706-81f9-911e2658c718"),
                   Name = "socks",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("ccb77e99-f5e6-496e-b972-e0e62b7a97c3"),
                   Name = "sweaters",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("ded76410-cbe7-4ab5-ae6f-f35fdaec41ea"),
                   Name = "jackets",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("f3e0a654-73f7-4a5f-b749-74a3b3f4d5ef"),
                   Name = "jewelry",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("f974b087-cf60-483e-b4db-1d5f84b24bcc"),
                   Name = "accessories",
                   Type = PreferenceType.Clothes
               },
               new PreferenceDm()
               {
                   Id = new Guid("f9e0d1f4-b7f6-4fb2-81f4-a44f5f0f77b0"),
                   Name = "t_shirts",
                   Type = PreferenceType.Clothes
               }
           );

        }
    }
}
