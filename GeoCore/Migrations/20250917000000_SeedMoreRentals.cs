using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeoCore.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreRentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rentals adicionales para edificios rentados y Huarte (BLD011)
            migrationBuilder.InsertData(
                table: "Rentals",
                columns: new[] { "RentalId", "ApartmentId", "StartDate", "EndDate", "IsConfirmed", "Price", "Zone", "PostalCode" },
                values: new object[,]
                {
                    // BLD002 (Edificio Diagonal, Barcelona)
                    { "REN100", "APT003", new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), true, 1400m, "Eixample", "08018" },
                    { "REN101", "APT004", new DateTime(2024, 2, 1), new DateTime(2024, 12, 31), true, 1350m, "Eixample", "08018" },
                    // BLD005 (Edificio Gran Vía, Bilbao)
                    { "REN102", "APT007", new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), true, 1500m, "Abando", "48009" },
                    // BLD009 (Edificio Maisonnave, Alicante) - si tienes un apartamento, por ejemplo APT010
                    { "REN103", "APT010", new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), true, 1100m, "Centro", "03003" },
                    // BLD011 (Huarte, Pamplona, Pérez Goyena) - usa APT031
                    { "REN104", "APT031", new DateTime(2024, 1, 1), new DateTime(2024, 12, 31), true, 1250m, "Huarte", "31012" }
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rentals",
                keyColumn: "RentalId",
                keyValues: new object[] { "REN100", "REN101", "REN102", "REN103", "REN104" }
            );
        }
    }
}