using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blabalacar.Migrations
{
    public partial class renameDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDateUser",
                table: "User",
                newName: "UserCreatedAt");

            migrationBuilder.RenameColumn(
                name: "DepartmentDate",
                table: "Trip",
                newName: "TripCreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreateDateTrip",
                table: "Trip",
                newName: "DepartureAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCreatedAt",
                table: "User",
                newName: "CreateDateUser");

            migrationBuilder.RenameColumn(
                name: "TripCreatedAt",
                table: "Trip",
                newName: "DepartmentDate");

            migrationBuilder.RenameColumn(
                name: "DepartureAt",
                table: "Trip",
                newName: "CreateDateTrip");
        }
    }
}
