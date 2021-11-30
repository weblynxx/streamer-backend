using Microsoft.EntityFrameworkCore.Migrations;

namespace streamer.db.Migrations
{
    public partial class AddFieldsAddressDeliveryFeilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Entrance",
                table: "Streamer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flat",
                table: "Streamer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Streamer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "House",
                table: "Streamer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseBuilding",
                table: "Streamer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entrance",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "Flat",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "House",
                table: "Streamer");

            migrationBuilder.DropColumn(
                name: "HouseBuilding",
                table: "Streamer");
        }
    }
}
