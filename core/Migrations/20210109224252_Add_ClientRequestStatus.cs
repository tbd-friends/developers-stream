using Microsoft.EntityFrameworkCore.Migrations;

namespace core.Migrations
{
    public partial class Add_ClientRequestStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StreamerClaimRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "StreamerClaimRequests");
        }
    }
}
