using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Drop_IsApprovedFromClaimRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "StreamerClaimRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "StreamerClaimRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
