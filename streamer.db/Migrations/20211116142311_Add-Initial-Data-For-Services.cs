using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddInitialDataForServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("bfab82b1-7cc6-4d37-8686-83da4b90e995"), "Twitch" },
                    { new Guid("83655a7a-92e0-4ce0-9d0c-f36fd5692a2a"), "Youtube" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: new Guid("83655a7a-92e0-4ce0-9d0c-f36fd5692a2a"));

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: new Guid("bfab82b1-7cc6-4d37-8686-83da4b90e995"));
        }
    }
}
