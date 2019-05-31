// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Softeq.NetKit.Profile.SQLRepository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    PhotoName = table.Column<string>(nullable: true),
                    Updated = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfile");
        }
    }
}