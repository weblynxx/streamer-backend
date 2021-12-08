using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddFieldsisStoppedDelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isStoppedDelivery",
                table: "Streamer",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6cc19d44-2ae4-467c-8f00-e597368f478b", "AQAAAAEAACcQAAAAEKV2up/EB5qZG47ZpJ+Lep+nJCR542psx/zBzRaHu/mOogwMlA6sXIcjMMi0JMM0SQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isStoppedDelivery",
                table: "Streamer");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9450beed-a4f9-4e2d-bc47-1f8bee85c541", "AQAAAAEAACcQAAAAELDYoDP7u0cIlyH6NA0caqJ4mMHIJ1q4jOUH/vTZPUvyo2oa8rCd+MtJHI2okzkOcQ==" });
        }
    }
}
