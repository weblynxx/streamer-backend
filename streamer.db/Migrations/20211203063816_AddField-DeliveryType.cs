using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddFieldDeliveryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Partner",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3a41a7e8-28a6-43ec-8101-6ef1e34872d0", "AQAAAAEAACcQAAAAEDdhfcBK/PL8LFMS3ymS6rmB+W/izNIjr3Uv2TJZigVzUqTAhLAMaNmc6qt7+e62eg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Partner");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8a1b3fbc-ab92-4eb6-85c7-484c6191e428", "AQAAAAEAACcQAAAAECK66qLDW7DpX/GKgicBVnn068KrDCrjnkoyL950xdlJDn1CjsAhrnxEyvl2sx3ckw==" });
        }
    }
}
