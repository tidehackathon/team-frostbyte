using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tide.Data.Ef.Migrations
{
    public partial class TcFaMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TcCount",
                table: "ObjectiveCycles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TcFail",
                table: "ObjectiveCycles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TcSuccess",
                table: "ObjectiveCycles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TcCount",
                table: "ObjectiveCycles");

            migrationBuilder.DropColumn(
                name: "TcFail",
                table: "ObjectiveCycles");

            migrationBuilder.DropColumn(
                name: "TcSuccess",
                table: "ObjectiveCycles");
        }
    }
}
