using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddTableStreamerPartner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamerPartner",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamerId = table.Column<Guid>(nullable: false),
                    PartnerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerPartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamerPartner_Partner_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamerPartner_Streamer_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b07effeb-0622-4274-a9b8-a51fc03557e2", "AQAAAAEAACcQAAAAEHmL/x0el/GvoxKRSAgLaaJEpg/VDEq3mIaABw9/heX22kuGe2HI2M4nHEBSPAWUZw==" });

            migrationBuilder.CreateIndex(
                name: "IX_StreamerPartner_PartnerId",
                table: "StreamerPartner",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamerPartner_StreamerId",
                table: "StreamerPartner",
                column: "StreamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamerPartner");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3331538e-c974-4cb7-9e4b-f99f6f0f9572", "AQAAAAEAACcQAAAAEKpHw0+R1f5uhD4GqpoHsHeW/JKQPGTzzmq7/4S/ZF7CDqQL+/5I0Zu8rh8Xcf7sZQ==" });
        }
    }
}
