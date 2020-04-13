using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleWorkOrder.Database.Migrations
{
    public partial class AddLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "LocationId",
                table: "WorkOrders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationDescription = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_LocationId",
                table: "WorkOrders",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Location_LocationId",
                table: "WorkOrders",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Location_LocationId",
                table: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_LocationId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "WorkOrders");
        }
    }
}
