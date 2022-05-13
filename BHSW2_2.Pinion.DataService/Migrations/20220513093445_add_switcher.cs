using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BHSW2_2.Pinion.DataService.Migrations
{
    public partial class add_switcher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SapConnectionSwitchers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapConnectionSwitchers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SapConnectionSwitchers");
        }
    }
}
