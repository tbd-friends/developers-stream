using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class AddEmailAndIsStreamer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Streamers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStreamer",
                table: "Streamers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Streamers");

            migrationBuilder.DropColumn(
                name: "IsStreamer",
                table: "Streamers");
        }
    }
}
