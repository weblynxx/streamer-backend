using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class Add_Tables_Service_StreamerService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Streamers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Streamers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Streamers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Streamers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Streamers",
                table: "Streamers");

            migrationBuilder.RenameTable(
                name: "Streamers",
                newName: "Streamer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Streamer",
                table: "Streamer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreamerService",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamerId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamerService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamerService_Streamer_StreamerId",
                        column: x => x.StreamerId,
                        principalTable: "Streamer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreamerService_ServiceId",
                table: "StreamerService",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamerService_StreamerId",
                table: "StreamerService",
                column: "StreamerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Streamer_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Streamer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Streamer_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Streamer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Streamer_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Streamer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Streamer_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Streamer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Streamer_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_Streamer_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Streamer_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_Streamer_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "StreamerService");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Streamer",
                table: "Streamer");

            migrationBuilder.RenameTable(
                name: "Streamer",
                newName: "Streamers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Streamers",
                table: "Streamers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Streamers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_Streamers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Streamers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_Streamers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "Streamers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
