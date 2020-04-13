using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleWorkOrder.Database.Migrations
{
    public partial class AddLastScannedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastScanned",
                table: "Cars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastScanned",
                table: "Cars");
        }
    }
}
