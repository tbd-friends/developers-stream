using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class AddDescriptionToStreamer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Streamers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Streamers");
        }
    }
}
