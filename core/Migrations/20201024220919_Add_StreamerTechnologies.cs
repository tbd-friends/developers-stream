using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_StreamerTechnologies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamerTechnologies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamerId = table.Column<Guid>(nullable: false),
                    TechnologyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerTechnologies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamerTechnologies");
        }
    }
}
