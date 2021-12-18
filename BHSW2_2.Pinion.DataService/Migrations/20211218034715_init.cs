using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BHSW2_2.Pinion.DataService.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SapRequestHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapRequestHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SapRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    SapRequestStatus = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Retries = table.Column<int>(nullable: false),
                    Error = table.Column<string>(nullable: true),
                    Created = table.Column<DateTimeOffset>(nullable: true),
                    Modified = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SapRequests", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SapRequestHistories");

            migrationBuilder.DropTable(
                name: "SapRequests");
        }
    }
}
