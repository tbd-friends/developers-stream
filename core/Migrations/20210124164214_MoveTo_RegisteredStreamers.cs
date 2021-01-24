using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class MoveTo_RegisteredStreamers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Streamers");

            migrationBuilder.CreateTable(
                name: "RegisteredStreamers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    StreamerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredStreamers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredStreamers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Streamers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
