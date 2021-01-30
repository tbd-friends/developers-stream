using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class AddOwnershipRequestDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "StreamerClaimRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "StreamerClaimRequests");
        }
    }
}
