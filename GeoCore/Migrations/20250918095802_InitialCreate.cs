using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GeoCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                name: "Rentals",
                columns: table => new
                {
                    RentalId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApartmentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.RentalId);
                });

            migrationBuilder.CreateTable(
                name: "Apartments",
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.ApartmentId);
                    table.ForeignKey(
                        name: "FK_Apartments_Buildings_BuildingId",
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

            migrationBuilder.InsertData(
                table: "Buildings",
                columns: new[] { "BuildingId", "Address", "BuildingCode", "City", "Latitude", "Longitude", "Name", "PostalCode", "PurchaseDate", "Status" },
                values: new object[,]
                {
                    { "BLD001", "Calle Mayor 1", "BLD001", "Madrid", 40.416977000000003, -3.7075230000000001, "Edificio Central", "28013", new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD002", "Carrer de Sardenya 350", "BLD002", "Barcelona", 41.404319999999998, 2.1740300000000001, "Edificio Diagonal", "08025", new DateTime(2021, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rented" },
                    { "BLD003", "Calle Feria 123", "BLD003", "Sevilla", 37.400649999999999, -5.9903500000000003, "Edificio Constitución", "41002", new DateTime(2022, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Under Maintenance" },
                    { "BLD004", "Avenida de Blasco Ibáñez 152", "BLD004", "Valencia", 39.474420000000002, -0.34734999999999999, "Edificio Paz", "46022", new DateTime(2019, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD005", "Calle Licenciado Poza 50", "BLD005", "Bilbao", 43.26379, -2.94347, "Edificio Gran Vía", "48011", new DateTime(2018, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rented" },
                    { "BLD006", "Calle Alhamar 28", "BLD006", "Granada", 37.172899999999998, -3.6038000000000001, "Edificio Reyes Católicos", "18004", new DateTime(2020, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD007", "Calle de San Vicente Mártir 55", "BLD007", "Zaragoza", 41.648589999999999, -0.88592000000000004, "Edificio Independencia", "50008", new DateTime(2021, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Under Maintenance" },
                    { "BLD008", "Calle Marqués de Larios 4", "BLD008", "Malaga", 36.719647999999999, -4.421265, "Edificio Larios", "29005", new DateTime(2019, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD009", "Calle Pintor Aparicio 16", "BLD009", "Alicante", 38.344900000000003, -0.49080000000000001, "Edificio Maisonnave", "03003", new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rented" },
                    { "BLD010", "Calle Estafeta 1", "BLD010", "Pamplona", 42.818452999999998, -1.6441319999999999, "Edificio Estafeta", "31001", new DateTime(2020, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD011", "Calle Pérez Goyena 28", "BLD011", "Pamplona", 42.800899999999999, -1.6168, "Edificio Pérez Goyena", "31620", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rented" },
                    { "BLD012", "Avenida de la Alameda 10", "BLD012", "Valencia", 39.474800000000002, -0.35699999999999998, "Edificio Alameda", "46023", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active" },
                    { "BLD013", "Calle Tomás Heredia 22", "BLD013", "Malaga", 36.715600000000002, -4.4231999999999996, "Edificio Soho", "29001", new DateTime(2022, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rented" }
                });

            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "RentalId", "ApartmentId", "EndDate", "IsConfirmed", "PostalCode", "Price", "StartDate", "Zone" },
                values: new object[,]
                {
                    { "REN001", "APT004", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "08025", 15000m, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eixample" },
                    { "REN002", "APT005", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "08025", 14000m, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eixample" },
                    { "REN003", "APT006", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "08025", 13000m, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Eixample" },
                    { "REN004", "APT013", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "48011", 16000m, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gran Via" },
                    { "REN005", "APT014", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "48011", 15500m, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gran Via" },
                    { "REN006", "APT015", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "48011", 15000m, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gran Via" },
                    { "REN007", "APT025", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "03003", 14000m, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Centro" },
                    { "REN008", "APT026", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "03003", 13500m, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Centro" },
                    { "REN009", "APT027", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "03003", 13000m, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Centro" },
                    { "REN010", "APT031", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "31620", 17000m, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iturrama" },
                    { "REN011", "APT032", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "31620", 16500m, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iturrama" },
                    { "REN020", "APT040", new DateTime(2022, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "46023", 12000m, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Camins al Grau" },
                    { "REN021", "APT050", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "29001", 15000m, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soho" }
                });

            migrationBuilder.InsertData(
                table: "Apartments",
                columns: new[] { "ApartmentId", "ApartmentDoor", "ApartmentFloor", "ApartmentPrice", "BuildingId", "CreatedDate", "HasGarage", "HasLift", "NumberOfBathrooms", "NumberOfRooms", "Status" },
                values: new object[,]
                {
                    { "APT001", "1A", "1", 120000m, "BLD001", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 2, 3, "ocupado" },
                    { "APT002", "1B", "1", 125000m, "BLD001", new DateTime(2023, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 1, 2, "libre" },
                    { "APT003", "2A", "2", 130000m, "BLD001", new DateTime(2023, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 2, 4, "reservado" },
                    { "APT004", "1A", "1", 110000m, "BLD002", new DateTime(2023, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 1, 2, "ocupado" },
                    { "APT005", "2A", "2", 115000m, "BLD002", new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 2, 3, "libre" },
                    { "APT006", "3A", "3", 118000m, "BLD002", new DateTime(2023, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 1, 2, "reservado" },
                    { "APT013", "1A", "1", 125000m, "BLD005", new DateTime(2023, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 2, 3, "ocupado" },
                    { "APT014", "2A", "2", 130000m, "BLD005", new DateTime(2023, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 2, 4, "libre" },
                    { "APT015", "3A", "3", 135000m, "BLD005", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, 1, 2, "reservado" },
                    { "APT031", "1A", "1", 150000m, "BLD011", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 2, 3, "ocupado" },
                    { "APT032", "2A", "2", 155000m, "BLD011", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 2, 3, "libre" },
                    { "APT040", "1A", "1", 185000m, "BLD012", new DateTime(2021, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 2, 3, "ocupado" },
                    { "APT041", "2B", "2", 175000m, "BLD012", new DateTime(2021, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, 1, 2, "libre" },
                    { "APT050", "1C", "1", 210000m, "BLD013", new DateTime(2022, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), true, true, 2, 4, "ocupado" },
                    { "APT051", "3A", "3", 160000m, "BLD013", new DateTime(2022, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, 1, 2, "reservado" }
                });

            migrationBuilder.InsertData(
                table: "CashFlows",
                columns: new[] { "CashFlowId", "Amount", "BuildingId", "Date", "Source" },
                values: new object[,]
                {
                    { "CAF001", 350000m, "BLD001", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF002", 20000m, "BLD001", new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF003", 5000m, "BLD001", new DateTime(2021, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reforma cocina" },
                    { "CAF004", 21000m, "BLD001", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF005", 1200m, "BLD001", new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seguro edificio" },
                    { "CAF006", 22000m, "BLD001", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF007", 320000m, "BLD002", new DateTime(2021, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF008", 18000m, "BLD002", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF009", 3000m, "BLD002", new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reforma baño" },
                    { "CAF010", 18500m, "BLD002", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF011", 950m, "BLD002", new DateTime(2023, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impuesto IBI" },
                    { "CAF012", 280000m, "BLD003", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF013", 17000m, "BLD003", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF014", 2500m, "BLD003", new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gasto extraordinario" },
                    { "CAF015", 250000m, "BLD004", new DateTime(2019, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF016", 16000m, "BLD004", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF017", 1100m, "BLD004", new DateTime(2020, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Limpieza fachada" },
                    { "CAF018", 400000m, "BLD005", new DateTime(2018, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF019", 19000m, "BLD005", new DateTime(2019, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF020", 4000m, "BLD005", new DateTime(2019, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reforma eléctrica" },
                    { "CAF021", 20000m, "BLD005", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF022", 950m, "BLD005", new DateTime(2020, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Impuesto IBI" },
                    { "CAF023", 210000m, "BLD006", new DateTime(2020, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF024", 14000m, "BLD006", new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF025", 90000m, "BLD007", new DateTime(2021, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF026", 95000m, "BLD008", new DateTime(2019, 9, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF027", 100000m, "BLD009", new DateTime(2022, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF028", 300000m, "BLD010", new DateTime(2020, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF029", 24000m, "BLD010", new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF030", 270000m, "BLD011", new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF031", 22000m, "BLD011", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF032", 1200m, "BLD011", new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seguro edificio" },
                    { "CAF040", 320000m, "BLD012", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF041", 12000m, "BLD012", new DateTime(2022, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF042", 2500m, "BLD012", new DateTime(2022, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reforma fachada" },
                    { "CAF050", 410000m, "BLD013", new DateTime(2022, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compra inicial" },
                    { "CAF051", 15000m, "BLD013", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Alquiler anual" },
                    { "CAF052", 1800m, "BLD013", new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gasto comunidad" }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceEvents",
                columns: new[] { "MaintenanceEventId", "BuildingId", "Cost", "Date", "Description" },
                values: new object[,]
                {
                    { "MAE001", "BLD001", 1000m, new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Revisión ascensor" },
                    { "MAE002", "BLD001", 1200m, new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cambio de caldera" },
                    { "MAE003", "BLD002", 900m, new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pintura fachada" },
                    { "MAE004", "BLD002", 1100m, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reparación tejado" },
                    { "MAE005", "BLD003", 1200m, new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cambio caldera" },
                    { "MAE006", "BLD004", 1100m, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reparación tejado" },
                    { "MAE007", "BLD005", 900m, new DateTime(2023, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Revisión eléctrica" },
                    { "MAE008", "BLD005", 1200m, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cambio caldera" },
                    { "MAE009", "BLD006", 800m, new DateTime(2023, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pintura fachada" },
                    { "MAE010", "BLD010", 1000m, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reparación portal" },
                    { "MAE011", "BLD011", 900m, new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reparación tejado ático" },
                    { "MAE020", "BLD012", 950m, new DateTime(2022, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Revisión ascensor" },
                    { "MAE021", "BLD013", 1100m, new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reparación portal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_BuildingId",
                table: "Apartments",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_CashFlows_BuildingId",
                table: "CashFlows",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceEvents_BuildingId",
                table: "MaintenanceEvents",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartments");

            migrationBuilder.DropTable(
                name: "CashFlows");

            migrationBuilder.DropTable(
                name: "MaintenanceEvents");

            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Buildings");
        }
    }
}
