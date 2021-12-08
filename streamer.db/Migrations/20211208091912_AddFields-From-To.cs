using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddFieldsFromTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "From",
                table: "Streamer",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "To",
                table: "Streamer",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9450beed-a4f9-4e2d-bc47-1f8bee85c541", "AQAAAAEAACcQAAAAELDYoDP7u0cIlyH6NA0caqJ4mMHIJ1q4jOUH/vTZPUvyo2oa8rCd+MtJHI2okzkOcQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "From",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Streamer");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3a41a7e8-28a6-43ec-8101-6ef1e34872d0", "AQAAAAEAACcQAAAAEDdhfcBK/PL8LFMS3ymS6rmB+W/izNIjr3Uv2TJZigVzUqTAhLAMaNmc6qt7+e62eg==" });
        }
    }
}
