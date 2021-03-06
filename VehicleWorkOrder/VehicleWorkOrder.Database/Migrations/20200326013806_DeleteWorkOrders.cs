﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleWorkOrder.Database.Migrations
{
    public partial class DeleteWorkOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "WorkOrders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "WorkOrders");
        }
    }
}
