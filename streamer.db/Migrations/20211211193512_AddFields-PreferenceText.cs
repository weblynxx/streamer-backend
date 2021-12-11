using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddFieldsPreferenceText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClothesPreferenceText",
                table: "Streamer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodPreferenceText",
                table: "Streamer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("026be9b4-beaa-45aa-876d-271aa539a8fb"),
                column: "Name",
                value: "pizza");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("0be66cc3-6804-4885-8acf-1b49f46b1a58"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "sushi", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("0cecd9ee-f8b0-46fc-92a3-e2c847f531fc"),
                column: "Name",
                value: "steaks");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("242cf954-086c-4c0f-9930-b22591ce978e"),
                column: "Name",
                value: "hot_dogs");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("43b0e39a-7a4e-4798-9249-e56cb188518f"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "vietnamese_food", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("5f453949-9525-408c-b558-889bfb52632a"),
                column: "Name",
                value: "georgian_food");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("6a1b34e4-24ac-46ba-b597-6104485b4bfd"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "seafood", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("71beecfb-66e5-4432-904f-e0033ae2b9e7"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "bakery_products", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("8bd2091c-78d2-4595-8679-ee5394357870"),
                column: "Name",
                value: "burgers");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("8d232f73-4430-4498-9b26-4490c083eb42"),
                column: "Name",
                value: "footwear");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("af250697-517c-4368-9e9c-aa6cba27eb33"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "head", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("b1920383-6d50-412f-860f-c5f255404930"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "trousers", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("b6f9de67-64c2-4706-81f9-911e2658c718"),
                column: "Name",
                value: "socks");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("ccb77e99-f5e6-496e-b972-e0e62b7a97c3"),
                column: "Name",
                value: "sweaters");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("ded76410-cbe7-4ab5-ae6f-f35fdaec41ea"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "jackets", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("f3e0a654-73f7-4a5f-b749-74a3b3f4d5ef"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "jewelry", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("f9e0d1f4-b7f6-4fb2-81f4-a44f5f0f77b0"),
                column: "Name",
                value: "t_shirts");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "08888f73-743c-4253-834c-4c5892374325", "AQAAAAEAACcQAAAAELJVo5qa2pm/uUNCoTVU8aan0o77YJ+f3vockCmMq11/4LRN813qcYlQzSi3iIL0QQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClothesPreferenceText",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "FoodPreferenceText",
                table: "Streamer");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("026be9b4-beaa-45aa-876d-271aa539a8fb"),
                column: "Name",
                value: "burgers");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("0be66cc3-6804-4885-8acf-1b49f46b1a58"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "sweaters", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("0cecd9ee-f8b0-46fc-92a3-e2c847f531fc"),
                column: "Name",
                value: "pizza");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("242cf954-086c-4c0f-9930-b22591ce978e"),
                column: "Name",
                value: "georgian_food");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("43b0e39a-7a4e-4798-9249-e56cb188518f"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "jackets", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("5f453949-9525-408c-b558-889bfb52632a"),
                column: "Name",
                value: "sushi");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("6a1b34e4-24ac-46ba-b597-6104485b4bfd"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "socks", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("71beecfb-66e5-4432-904f-e0033ae2b9e7"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "jewelry", 1 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("8bd2091c-78d2-4595-8679-ee5394357870"),
                column: "Name",
                value: "steaks");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("8d232f73-4430-4498-9b26-4490c083eb42"),
                column: "Name",
                value: "t_shirts");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("af250697-517c-4368-9e9c-aa6cba27eb33"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "hot_dogs", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("b1920383-6d50-412f-860f-c5f255404930"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "bakery_products", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("b6f9de67-64c2-4706-81f9-911e2658c718"),
                column: "Name",
                value: "footwear");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("ccb77e99-f5e6-496e-b972-e0e62b7a97c3"),
                column: "Name",
                value: "head");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("ded76410-cbe7-4ab5-ae6f-f35fdaec41ea"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "vietnamese_food", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("f3e0a654-73f7-4a5f-b749-74a3b3f4d5ef"),
                columns: new[] { "Name", "Type" },
                values: new object[] { "seafood", 0 });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: new Guid("f9e0d1f4-b7f6-4fb2-81f4-a44f5f0f77b0"),
                column: "Name",
                value: "trousers");

            migrationBuilder.UpdateData(
                table: "Streamer",
                keyColumn: "Id",
                keyValue: new Guid("cf111dde-367b-4528-8558-3d753371ce34"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7729a4cd-f863-4d70-b5bc-caf8f4400fca", "AQAAAAEAACcQAAAAEJes6qf/f3fglxSoG3Wxn00yR9a0UHA6O3f9AGzoqgDMwwqPre1DY1gtrkZAvspwrg==" });
        }
    }
}
