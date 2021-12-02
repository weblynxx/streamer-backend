using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddTable_Partner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partner",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeliveryName = table.Column<string>(nullable: true),
                    StreamerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partner_Streamer_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8a1b3fbc-ab92-4eb6-85c7-484c6191e428", "AQAAAAEAACcQAAAAECK66qLDW7DpX/GKgicBVnn068KrDCrjnkoyL950xdlJDn1CjsAhrnxEyvl2sx3ckw==" });

            migrationBuilder.CreateIndex(
                name: "IX_Partner_StreamerId",
                table: "Partner",
                column: "StreamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partner");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e5a21b03-ea96-4cf8-8d02-13ee673c7d9d", "AQAAAAEAACcQAAAAEM7S9LsgsHiJ95uVMfsE6Gc5ADJk7/WWD3SBGlL7tz2MP4gE/StdBK5+O6gXcAkj8w==" });
        }
    }
}
