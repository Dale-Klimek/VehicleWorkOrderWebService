using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VehicleWorkOrder.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufactures",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufactureName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufactures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technicians", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<short>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufactureId = table.Column<short>(nullable: true),
                    ModelName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Manufactures_ManufactureId",
                        column: x => x.ManufactureId,
                        principalTable: "Manufactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleIdentification = table.Column<string>(type: "varchar(17)", maxLength: 17, nullable: true),
                    ModelId = table.Column<short>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Series = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Trim = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Doors = table.Column<int>(nullable: true),
                    VehicleType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    TransmissionStyle = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ScannedDate = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(nullable: false),
                    WorkOrderNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    PurchaseOrder = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    TechnicianId = table.Column<short>(nullable: true),
                    RepairOrder = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    FeatureAdded = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UserAddedId = table.Column<int>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkOrders_Users_UserAddedId",
                        column: x => x.UserAddedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ModelId",
                table: "Cars",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_ManufactureId",
                table: "Models",
                column: "ManufactureId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CarId",
                table: "WorkOrders",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_TechnicianId",
                table: "WorkOrders",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_UserAddedId",
                table: "WorkOrders",
                column: "UserAddedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Manufactures");
        }
    }
}
