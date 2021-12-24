using Microsoft.EntityFrameworkCore.Migrations;

namespace BHSW2_2.Pinion.DataService.Migrations
{
    public partial class addProcessOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessOrder",
                table: "SapRequests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessOrder",
                table: "SapRequests");
        }
    }
}
