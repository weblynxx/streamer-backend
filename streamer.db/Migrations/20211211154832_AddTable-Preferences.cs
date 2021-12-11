using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddTablePreferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("0cecd9ee-f8b0-46fc-92a3-e2c847f531fc"), "pizza", 0 },
                    { new Guid("f974b087-cf60-483e-b4db-1d5f84b24bcc"), "accessories", 1 },
                    { new Guid("71beecfb-66e5-4432-904f-e0033ae2b9e7"), "jewelry", 1 },
                    { new Guid("43b0e39a-7a4e-4798-9249-e56cb188518f"), "jackets", 1 },
                    { new Guid("0be66cc3-6804-4885-8acf-1b49f46b1a58"), "sweaters", 1 },
                    { new Guid("6a1b34e4-24ac-46ba-b597-6104485b4bfd"), "socks", 1 },
                    { new Guid("f9e0d1f4-b7f6-4fb2-81f4-a44f5f0f77b0"), "trousers", 1 },
                    { new Guid("ccb77e99-f5e6-496e-b972-e0e62b7a97c3"), "head", 1 },
                    { new Guid("8d232f73-4430-4498-9b26-4490c083eb42"), "t_shirts", 1 },
                    { new Guid("b6f9de67-64c2-4706-81f9-911e2658c718"), "footwear", 1 },
                    { new Guid("b1920383-6d50-412f-860f-c5f255404930"), "bakery_products", 0 },
                    { new Guid("f3e0a654-73f7-4a5f-b749-74a3b3f4d5ef"), "seafood", 0 },
                    { new Guid("242cf954-086c-4c0f-9930-b22591ce978e"), "georgian_food", 0 },
                    { new Guid("ded76410-cbe7-4ab5-ae6f-f35fdaec41ea"), "vietnamese_food", 0 },
                    { new Guid("af250697-517c-4368-9e9c-aa6cba27eb33"), "hot_dogs", 0 },
                    { new Guid("8bd2091c-78d2-4595-8679-ee5394357870"), "steaks", 0 },
                    { new Guid("5f453949-9525-408c-b558-889bfb52632a"), "sushi", 0 },
                    { new Guid("026be9b4-beaa-45aa-876d-271aa539a8fb"), "burgers", 0 }
                });

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7729a4cd-f863-4d70-b5bc-caf8f4400fca", "AQAAAAEAACcQAAAAEJes6qf/f3fglxSoG3Wxn00yR9a0UHA6O3f9AGzoqgDMwwqPre1DY1gtrkZAvspwrg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6cc19d44-2ae4-467c-8f00-e597368f478b", "AQAAAAEAACcQAAAAEKV2up/EB5qZG47ZpJ+Lep+nJCR542psx/zBzRaHu/mOogwMlA6sXIcjMMi0JMM0SQ==" });
        }
    }
}
