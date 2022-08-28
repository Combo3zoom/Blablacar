using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blabalacar.Migrations
{
    public partial class addColumnTripIdInRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Route");
        }
    }
}
