using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_ProfileIdToRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "StreamerClaimRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "StreamerClaimRequests");
        }
    }
}
