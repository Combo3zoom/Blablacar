using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blabalacar.Migrations
{
    public partial class smt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips");

            migrationBuilder.DropIndex(
                name: "IX_UserTrips_TripId",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTrips");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateUser",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDateTrip",
                table: "Trip",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "StartRoute",
                table: "Route",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EndRoute",
                table: "Route",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips",
                columns: new[] { "TripId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips");

            migrationBuilder.DropColumn(
                name: "CreateDateUser",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreateDateTrip",
                table: "Trip");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserTrips",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "StartRoute",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "EndRoute",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrips",
                table: "UserTrips",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrips_TripId",
                table: "UserTrips",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Route_RouteId",
                table: "Trip",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
