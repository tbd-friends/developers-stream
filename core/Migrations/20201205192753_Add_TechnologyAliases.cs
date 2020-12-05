using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_TechnologyAliases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aliases",
                table: "AvailableTechnologies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aliases",
                table: "AvailableTechnologies");
        }
    }
}
