using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_StreamerPlatforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamerPlatforms",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StreamerId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerPlatforms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamerPlatforms");
        }
    }
}
