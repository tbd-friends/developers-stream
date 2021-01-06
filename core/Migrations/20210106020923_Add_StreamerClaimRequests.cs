using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_StreamerClaimRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamerClaimRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CurrentEmail = table.Column<string>(nullable: true),
                    UpdatedEmail = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Updated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamerClaimRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamerClaimRequests");
        }
    }
}
