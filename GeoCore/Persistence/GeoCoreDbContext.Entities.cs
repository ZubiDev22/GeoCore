using GeoCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GeoCore.Persistence
{
    public partial class GeoCoreDbContext : DbContext
    {
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<MaintenanceEvent> MaintenanceEvents { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CashFlow>()
                .Property(c => c.Amount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<MaintenanceEvent>()
                .Property(m => m.Cost)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Rental>()
                .Property(r => r.Price)
                .HasPrecision(10, 2);

            // SEED: Buildings (11 edificios reales)
            modelBuilder.Entity<Building>().HasData(
                new Building { BuildingId = "BLD001", BuildingCode = "BLD001", Name = "Edificio Central", Address = "Calle Mayor 1", City = "Madrid", Latitude = 40.416977, Longitude = -3.707523, PurchaseDate = DateTime.Parse("2020-01-01"), Status = "Active", PostalCode = "28013" },
                new Building { BuildingId = "BLD002", BuildingCode = "BLD002", Name = "Edificio Diagonal", Address = "Carrer de Sardenya 350", City = "Barcelona", Latitude = 41.404320, Longitude = 2.174030, PurchaseDate = DateTime.Parse("2021-05-15"), Status = "Rented", PostalCode = "08025" },
                new Building { BuildingId = "BLD003", BuildingCode = "BLD003", Name = "Edificio Constitución", Address = "Calle Feria 123", City = "Sevilla", Latitude = 37.400650, Longitude = -5.990350, PurchaseDate = DateTime.Parse("2022-03-10"), Status = "Under Maintenance", PostalCode = "41002" },
                new Building { BuildingId = "BLD004", BuildingCode = "BLD004", Name = "Edificio Paz", Address = "Avenida de Blasco Ibáñez 152", City = "Valencia", Latitude = 39.474420, Longitude = -0.347350, PurchaseDate = DateTime.Parse("2019-07-22"), Status = "Active", PostalCode = "46022" },
                new Building { BuildingId = "BLD005", BuildingCode = "BLD005", Name = "Edificio Gran Vía", Address = "Calle Licenciado Poza 50", City = "Bilbao", Latitude = 43.263790, Longitude = -2.943470, PurchaseDate = DateTime.Parse("2018-11-30"), Status = "Rented", PostalCode = "48011" },
                new Building { BuildingId = "BLD006", BuildingCode = "BLD006", Name = "Edificio Reyes Católicos", Address = "Calle Alhamar 28", City = "Granada", Latitude = 37.172900, Longitude = -3.603800, PurchaseDate = DateTime.Parse("2020-06-15"), Status = "Active", PostalCode = "18004" },
                new Building { BuildingId = "BLD007", BuildingCode = "BLD007", Name = "Edificio Independencia", Address = "Calle de San Vicente Mártir 55", City = "Zaragoza", Latitude = 41.648590, Longitude = -0.885920, PurchaseDate = DateTime.Parse("2021-02-20"), Status = "Under Maintenance", PostalCode = "50008" },
                new Building { BuildingId = "BLD008", BuildingCode = "BLD008", Name = "Edificio Larios", Address = "Calle Marqués de Larios 4", City = "Malaga", Latitude = 36.719648, Longitude = -4.421265, PurchaseDate = DateTime.Parse("2019-09-10"), Status = "Active", PostalCode = "29005" },
                new Building { BuildingId = "BLD009", BuildingCode = "BLD009", Name = "Edificio Maisonnave", Address = "Calle Pintor Aparicio 16", City = "Alicante", Latitude = 38.344900, Longitude = -0.490800, PurchaseDate = DateTime.Parse("2022-01-05"), Status = "Rented", PostalCode = "03003" },
                new Building { BuildingId = "BLD010", BuildingCode = "BLD010", Name = "Edificio Estafeta", Address = "Calle Estafeta 1", City = "Pamplona", Latitude = 42.818453, Longitude = -1.644132, PurchaseDate = DateTime.Parse("2020-03-18"), Status = "Active", PostalCode = "31001" },
                new Building { BuildingId = "BLD011", BuildingCode = "BLD011", Name = "Edificio Pérez Goyena", Address = "Calle Pérez Goyena 28", City = "Pamplona", Latitude = 42.800900, Longitude = -1.616800, PurchaseDate = DateTime.Parse("2023-01-01"), Status = "Rented", PostalCode = "31620" }
            );

            // SEED: Apartments (35+ apartamentos reales, conectados)
            modelBuilder.Entity<Apartment>().HasData(
                new Apartment { ApartmentId = "APT001", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 120000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD001", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-01"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT002", ApartmentDoor = "1B", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD001", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-02"), Status = "libre" },
                new Apartment { ApartmentId = "APT003", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 130000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD001", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-03"), Status = "reservado" },
                new Apartment { ApartmentId = "APT004", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 110000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD002", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-04"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT005", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 115000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD002", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-05"), Status = "libre" },
                new Apartment { ApartmentId = "APT006", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 118000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD002", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-06"), Status = "reservado" },
                new Apartment { ApartmentId = "APT013", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD005", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-13"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT014", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 130000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD005", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-14"), Status = "libre" },
                new Apartment { ApartmentId = "APT015", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 135000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD005", HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-15"), Status = "reservado" },
                new Apartment { ApartmentId = "APT031", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 150000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-02-01"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT032", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 155000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-02-02"), Status = "libre" }
                // ...añade aquí el resto de apartamentos de los stubs...
            );

            // SEED: Rentals (más ingresos para edificios rentados, todos confirmados y conectados, zonas reales)
            modelBuilder.Entity<Rental>().HasData(
                // BLD002 (Barcelona, Eixample)
                new Rental { RentalId = "REN001", ApartmentId = "APT004", StartDate = DateTime.Parse("2023-01-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 15000, Zone = "Eixample", PostalCode = "08025" },
                new Rental { RentalId = "REN002", ApartmentId = "APT005", StartDate = DateTime.Parse("2023-02-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 14000, Zone = "Eixample", PostalCode = "08025" },
                new Rental { RentalId = "REN003", ApartmentId = "APT006", StartDate = DateTime.Parse("2023-03-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 13000, Zone = "Eixample", PostalCode = "08025" },
                // BLD005 (Bilbao, Gran Via)
                new Rental { RentalId = "REN004", ApartmentId = "APT013", StartDate = DateTime.Parse("2023-01-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 16000, Zone = "Gran Via", PostalCode = "48011" },
                new Rental { RentalId = "REN005", ApartmentId = "APT014", StartDate = DateTime.Parse("2023-02-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 15500, Zone = "Gran Via", PostalCode = "48011" },
                new Rental { RentalId = "REN006", ApartmentId = "APT015", StartDate = DateTime.Parse("2023-03-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 15000, Zone = "Gran Via", PostalCode = "48011" },
                // BLD009 (Alicante, Centro)
                new Rental { RentalId = "REN007", ApartmentId = "APT025", StartDate = DateTime.Parse("2023-01-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 14000, Zone = "Centro", PostalCode = "03003" },
                new Rental { RentalId = "REN008", ApartmentId = "APT026", StartDate = DateTime.Parse("2023-02-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 13500, Zone = "Centro", PostalCode = "03003" },
                new Rental { RentalId = "REN009", ApartmentId = "APT027", StartDate = DateTime.Parse("2023-03-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 13000, Zone = "Centro", PostalCode = "03003" },
                // BLD011 (Pamplona, Iturrama)
                new Rental { RentalId = "REN010", ApartmentId = "APT031", StartDate = DateTime.Parse("2023-01-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 17000, Zone = "Iturrama", PostalCode = "31620" },
                new Rental { RentalId = "REN011", ApartmentId = "APT032", StartDate = DateTime.Parse("2023-02-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 16500, Zone = "Iturrama", PostalCode = "31620" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
