﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using streamer.db;

namespace streamer.db.Migrations
{
    [DbContext(typeof(StreamerDbContext))]
    partial class StreamerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.PartnerDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("DeliveryName");

                    b.Property<Guid?>("StreamerId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("StreamerId");

                    b.ToTable("Partner");
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.PreferenceDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Preference");

                    b.HasData(
                        new
                        {
                            Id = new Guid("026be9b4-beaa-45aa-876d-271aa539a8fb"),
                            Name = "pizza",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("0be66cc3-6804-4885-8acf-1b49f46b1a58"),
                            Name = "sushi",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("0cecd9ee-f8b0-46fc-92a3-e2c847f531fc"),
                            Name = "steaks",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("242cf954-086c-4c0f-9930-b22591ce978e"),
                            Name = "hot_dogs",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("43b0e39a-7a4e-4798-9249-e56cb188518f"),
                            Name = "vietnamese_food",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("5f453949-9525-408c-b558-889bfb52632a"),
                            Name = "georgian_food",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("6a1b34e4-24ac-46ba-b597-6104485b4bfd"),
                            Name = "seafood",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("71beecfb-66e5-4432-904f-e0033ae2b9e7"),
                            Name = "bakery_products",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("8bd2091c-78d2-4595-8679-ee5394357870"),
                            Name = "burgers",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("8d232f73-4430-4498-9b26-4490c083eb42"),
                            Name = "footwear",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("af250697-517c-4368-9e9c-aa6cba27eb33"),
                            Name = "head",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("b1920383-6d50-412f-860f-c5f255404930"),
                            Name = "trousers",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("b6f9de67-64c2-4706-81f9-911e2658c718"),
                            Name = "socks",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("ccb77e99-f5e6-496e-b972-e0e62b7a97c3"),
                            Name = "sweaters",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("ded76410-cbe7-4ab5-ae6f-f35fdaec41ea"),
                            Name = "jackets",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("f3e0a654-73f7-4a5f-b749-74a3b3f4d5ef"),
                            Name = "jewelry",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("f974b087-cf60-483e-b4db-1d5f84b24bcc"),
                            Name = "accessories",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("f9e0d1f4-b7f6-4fb2-81f4-a44f5f0f77b0"),
                            Name = "t_shirts",
                            Type = 1
                        });
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.RefreshTokenDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Expires");

                    b.Property<string>("RemoteIpAddress");

                    b.Property<string>("Token")
                        .HasMaxLength(32);

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.ServiceDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Service");

                    b.HasData(
                        new
                        {
                            Id = new Guid("bfab82b1-7cc6-4d37-8686-83da4b90e995"),
                            Name = "Twitch"
                        },
                        new
                        {
                            Id = new Guid("83655a7a-92e0-4ce0-9d0c-f36fd5692a2a"),
                            Name = "Youtube"
                        });
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.StreamerDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Authorities");

                    b.Property<string>("City");

                    b.Property<string>("ClothesPreferenceText");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Entrance");

                    b.Property<string>("FirstName");

                    b.Property<string>("Flat");

                    b.Property<int>("Floor");

                    b.Property<string>("FoodPreferenceText");

                    b.Property<TimeSpan>("From");

                    b.Property<string>("House");

                    b.Property<string>("HouseBuilding");

                    b.Property<string>("IntercomCode");

                    b.Property<DateTime>("LastLoginDate");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("Phone");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<Guid>("StreamerId");

                    b.Property<string>("Street");

                    b.Property<TimeSpan>("To");

                    b.Property<string>("Token");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<bool>("isStoppedDelivery");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Streamer");

                    b.HasData(
                        new
                        {
                            Id = new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                            AccessFailedCount = 0,
                            Authorities = "ROLE_ADMIN",
                            ConcurrencyStamp = "08888f73-743c-4253-834c-4c5892374325",
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "John",
                            Floor = 0,
                            From = new TimeSpan(0, 0, 0, 0, 0),
                            LastLoginDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAELJVo5qa2pm/uUNCoTVU8aan0o77YJ+f3vockCmMq11/4LRN813qcYlQzSi3iIL0QQ==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "09355be4-a18d-41a9-acce-dd48c68211d7",
                            StreamerId = new Guid("00000000-0000-0000-0000-000000000000"),
                            To = new TimeSpan(0, 0, 0, 0, 0),
                            TwoFactorEnabled = false,
                            UserName = "admin",
                            isStoppedDelivery = false
                        });
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.StreamerServiceDm", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ServiceId");

                    b.Property<string>("ServiceUserName");

                    b.Property<Guid>("StreamerId");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.HasIndex("StreamerId");

                    b.ToTable("StreamerService");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("streamer.db.Database.DataModel.StreamerDm")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("streamer.db.Database.DataModel.StreamerDm")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("streamer.db.Database.DataModel.StreamerDm")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("streamer.db.Database.DataModel.StreamerDm")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.PartnerDm", b =>
                {
                    b.HasOne("streamer.db.Database.DataModel.StreamerDm", "Streamer")
                        .WithMany()
                        .HasForeignKey("StreamerId");
                });

            modelBuilder.Entity("streamer.db.Database.DataModel.StreamerServiceDm", b =>
                {
                    b.HasOne("streamer.db.Database.DataModel.ServiceDm", "Service")
                        .WithMany("StreamerServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("streamer.db.Database.DataModel.StreamerDm", "Streamer")
                        .WithMany("StreamerServices")
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
