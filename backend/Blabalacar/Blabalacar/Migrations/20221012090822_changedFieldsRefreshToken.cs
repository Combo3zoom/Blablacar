using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blabalacar.Migrations
{
    public partial class changedFieldsRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "TokenExpires",
                table: "AspNetUsers",
                newName: "RefreshTokenExpiresAt");

            migrationBuilder.RenameColumn(
                name: "TokenCreated",
                table: "AspNetUsers",
                newName: "RefreshTokenCreatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiresAt",
                table: "AspNetUsers",
                newName: "TokenExpires");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenCreatedAt",
                table: "AspNetUsers",
                newName: "TokenCreated");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Expires = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });
        }
    }
}
