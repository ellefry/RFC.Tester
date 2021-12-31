using Microsoft.EntityFrameworkCore.Migrations;

namespace BHSW2_2.Pinion.DataService.Migrations
{
    public partial class addSapMessageToHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SapMessage",
                table: "SapRequestHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SapMessage",
                table: "SapRequestHistories");
        }
    }
}
