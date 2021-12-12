using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddTableStreamerPreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamerPreference",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamerId = table.Column<Guid>(nullable: false),
                    PreferenceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamerPreference_Preference_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "Preference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StreamerPreference_Streamer_StreamerId",
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
                values: new object[] { "3331538e-c974-4cb7-9e4b-f99f6f0f9572", "AQAAAAEAACcQAAAAEKpHw0+R1f5uhD4GqpoHsHeW/JKQPGTzzmq7/4S/ZF7CDqQL+/5I0Zu8rh8Xcf7sZQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_StreamerPreference_PreferenceId",
                table: "StreamerPreference",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_StreamerPreference_StreamerId",
                table: "StreamerPreference",
                column: "StreamerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamerPreference");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "08888f73-743c-4253-834c-4c5892374325", "AQAAAAEAACcQAAAAELJVo5qa2pm/uUNCoTVU8aan0o77YJ+f3vockCmMq11/4LRN813qcYlQzSi3iIL0QQ==" });
        }
    }
}
