using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    ApartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApartmentDoor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentFloor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfRooms = table.Column<int>(type: "int", nullable: false),
                    NumberOfBathrooms = table.Column<int>(type: "int", nullable: false),
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasLift = table.Column<bool>(type: "bit", nullable: false),
                    HasGarage = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartment_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashFlows",
                columns: table => new
                {
                    CashFlowId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashFlows", x => x.CashFlowId);
                    table.ForeignKey(
                        name: "FK_CashFlows_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceEvents",
                columns: table => new
                {
                    MaintenanceEventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceEvents", x => x.MaintenanceEventId);
                    table.ForeignKey(
                        name: "FK_MaintenanceEvents_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartment_BuildingId",
                table: "Apartment",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_BuildingId",
                table: "CashFlows",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceEvents_BuildingId",
                table: "MaintenanceEvents",
                column: "BuildingId");

            // Seed Buildings
            migrationBuilder.Sql(@"
                INSERT INTO Buildings (BuildingId, BuildingCode, Name, Address, City, Latitude, Longitude, PurchaseDate, Status, PostalCode) VALUES
                ('BLD001', 'BLD001', 'Edificio Central', 'Calle Mayor 1', 'Madrid', 40.416977, -3.707523, '2020-01-01', 'Active', '28013'),
                ('BLD002', 'BLD002', 'Edificio Diagonal', 'Avinguda Diagonal 211', 'Barcelona', 41.394897, 2.140408, '2021-05-15', 'Rented', '08018'),
                ('BLD003', 'BLD003', 'Edificio Constitución', 'Avenida de la Constitución 1', 'Sevilla', 37.388797, -5.994461, '2022-03-10', 'Under Maintenance', '41004'),
                ('BLD004', 'BLD004', 'Edificio Paz', 'Calle de la Paz 15', 'Valencia', 39.473889, -0.375278, '2019-07-22', 'Active', '46003'),
                ('BLD005', 'BLD005', 'Edificio Gran Vía', 'Gran Vía 20', 'Bilbao', 43.263012, -2.935003, '2018-11-30', 'Rented', '48009'),
                ('BLD006', 'BLD006', 'Edificio Reyes Católicos', 'Calle Reyes Católicos 17', 'Granada', 37.176487, -3.599556, '2020-06-15', 'Active', '18009'),
                ('BLD007', 'BLD007', 'Edificio Independencia', 'Paseo Independencia 24', 'Zaragoza', 41.649693, -0.887712, '2021-02-20', 'Under Maintenance', '50001'),
                ('BLD008', 'BLD008', 'Edificio Larios', 'Calle Marqués de Larios 4', 'Malaga', 36.719648, -4.421265, '2019-09-10', 'Active', '29005'),
                ('BLD009', 'BLD009', 'Edificio Maisonnave', 'Avenida Maisonnave 41', 'Alicante', 38.345170, -0.483034, '2022-01-05', 'Rented', '03003'),
                ('BLD010', 'BLD010', 'Edificio Estafeta', 'Calle Estafeta 1', 'Pamplona', 42.818453, -1.644132, '2020-03-18', 'Active', '31001')
            ");

            // Seed Apartments
            migrationBuilder.Sql(@"
                INSERT INTO Apartment (ApartmentId, ApartmentDoor, ApartmentFloor, ApartmentPrice, NumberOfRooms, NumberOfBathrooms, BuildingId, HasLift, HasGarage, CreatedDate) VALUES
                ('APT001', 'A', '1', 1200, 3, 2, 'BLD001', 1, 1, '2020-01-02'),
                ('APT002', 'B', '2', 1100, 2, 1, 'BLD001', 1, 0, '2020-01-03'),
                ('APT003', 'A', '1', 1000, 2, 1, 'BLD002', 1, 1, '2021-05-16'),
                ('APT004', 'B', '2', 950, 1, 1, 'BLD002', 0, 0, '2021-05-17'),
                ('APT005', 'A', '1', 900, 2, 1, 'BLD003', 1, 0, '2022-03-11'),
                ('APT006', 'A', '1', 850, 2, 1, 'BLD004', 1, 1, '2019-07-23'),
                ('APT007', 'A', '1', 950, 3, 2, 'BLD005', 1, 1, '2018-12-01'),
                ('APT008', 'A', '1', 700, 1, 1, 'BLD006', 0, 0, '2020-06-16'),
                ('APT009', 'A', '1', 800, 2, 1, 'BLD007', 1, 0, '2021-02-21'),
                ('APT010', 'A', '1', 750, 2, 1, 'BLD008', 1, 1, '2019-09-11')
            ");

            // Seed CashFlows
            migrationBuilder.Sql(@"
                INSERT INTO CashFlows (CashFlowId, BuildingId, Date, Amount, Source) VALUES
                ('CF001', 'BLD001', '2020-01-10', 100000, 'Compra'),
                ('CF002', 'BLD002', '2021-05-20', 95000, 'Compra'),
                ('CF003', 'BLD003', '2022-03-15', 80000, 'Compra'),
                ('CF004', 'BLD004', '2019-07-25', 120000, 'Compra'),
                ('CF005', 'BLD005', '2018-12-05', 110000, 'Compra'),
                ('CF006', 'BLD006', '2020-06-20', 90000, 'Compra'),
                ('CF007', 'BLD007', '2021-02-25', 85000, 'Compra'),
                ('CF008', 'BLD008', '2019-09-15', 105000, 'Compra'),
                ('CF009', 'BLD009', '2022-01-10', 97000, 'Compra'),
                ('CF010', 'BLD010', '2020-03-20', 82000, 'Compra')
            ");

            // Seed MaintenanceEvents
            migrationBuilder.Sql(@"
                INSERT INTO MaintenanceEvents (MaintenanceEventId, BuildingId, Date, Description, Cost) VALUES
                ('ME001', 'BLD001', '2020-06-01', 'Reparación ascensor', 2000),
                ('ME002', 'BLD002', '2021-08-15', 'Pintura fachada', 1500),
                ('ME003', 'BLD003', '2022-09-10', 'Cambio caldera', 1800),
                ('ME004', 'BLD004', '2019-11-05', 'Reforma portal', 2500),
                ('ME005', 'BLD005', '2018-12-20', 'Impermeabilización tejado', 3000),
                ('ME006', 'BLD006', '2021-01-15', 'Reparación tuberías', 1200),
                ('ME007', 'BLD007', '2021-05-10', 'Sustitución ventanas', 1700),
                ('ME008', 'BLD008', '2020-02-20', 'Reforma garaje', 2200),
                ('ME009', 'BLD009', '2022-02-15', 'Reparación fachada', 1600),
                ('ME010', 'BLD010', '2020-03-25', 'Cambio ascensor', 2100)
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "CashFlows");

            migrationBuilder.DropTable(
                name: "MaintenanceEvents");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
