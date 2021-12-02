using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddInitAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Streamer",
                columns: new[] { "Id", "AccessFailedCount", "Authorities", "City", "ConcurrencyStamp", "CreatedDate", "Email", "EmailConfirmed", "Entrance", "FirstName", "Flat", "Floor", "House", "HouseBuilding", "IntercomCode", "LastLoginDate", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "Phone", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StreamerId", "Street", "Token", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("cf111dde-367b-4528-8558-3d753371ce34"), 0, "ROLE_ADMIN", null, "e5a21b03-ea96-4cf8-8d02-13ee673c7d9d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, "John", null, 0, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", false, null, null, "ADMIN", null, "AQAAAAEAACcQAAAAEM7S9LsgsHiJ95uVMfsE6Gc5ADJk7/WWD3SBGlL7tz2MP4gE/StdBK5+O6gXcAkj8w==", null, null, false, "09355be4-a18d-41a9-acce-dd48c68211d7", new Guid("00000000-0000-0000-0000-000000000000"), null, null, false, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"));
        }
    }
}
