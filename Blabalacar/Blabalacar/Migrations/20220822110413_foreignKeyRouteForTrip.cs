using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blabalacar.Migrations
{
    public partial class foreignKeyRouteForTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
