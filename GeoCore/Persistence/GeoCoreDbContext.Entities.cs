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
                new Building { BuildingId = "BLD011", BuildingCode = "BLD011", Name = "Edificio Pérez Goyena", Address = "Calle Pérez Goyena 28", City = "Pamplona", Latitude = 42.800900, Longitude = -1.616800, PurchaseDate = DateTime.Parse("2023-01-01"), Status = "Rented", PostalCode = "31620" },
                // ...edificios existentes...
                new Building { BuildingId = "BLD012", BuildingCode = "BLD012", Name = "Edificio Alameda", Address = "Avenida de la Alameda 10", City = "Valencia", Latitude = 39.474800, Longitude = -0.357000, PurchaseDate = DateTime.Parse("2021-03-15"), Status = "Active", PostalCode = "46023" },
                new Building { BuildingId = "BLD013", BuildingCode = "BLD013", Name = "Edificio Soho", Address = "Calle Tomás Heredia 22", City = "Malaga", Latitude = 36.715600, Longitude = -4.423200, PurchaseDate = DateTime.Parse("2022-05-10"), Status = "Rented", PostalCode = "29001" }
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
                new Apartment { ApartmentId = "APT032", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 155000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-02-02"), Status = "libre" },
                // Añadidos apartamentos para los nuevos edificios
                new Apartment { ApartmentId = "APT040", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 185000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD012", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2021-03-16"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT041", ApartmentDoor = "2B", ApartmentFloor = "2", ApartmentPrice = 175000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD012", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2021-03-17"), Status = "libre" },
                new Apartment { ApartmentId = "APT050", ApartmentDoor = "1C", ApartmentFloor = "1", ApartmentPrice = 210000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD013", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2022-05-11"), Status = "ocupado" },
                new Apartment { ApartmentId = "APT051", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 160000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD013", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2022-05-12"), Status = "reservado" }
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
                new Rental { RentalId = "REN011", ApartmentId = "APT032", StartDate = DateTime.Parse("2023-02-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 16500, Zone = "Iturrama", PostalCode = "31620" },
                // Añadidos rentals para los nuevos apartamentos
                new Rental { RentalId = "REN020", ApartmentId = "APT040", StartDate = DateTime.Parse("2022-01-01"), EndDate = DateTime.Parse("2022-12-31"), IsConfirmed = true, Price = 12000, Zone = "Camins al Grau", PostalCode = "46023" },
                new Rental { RentalId = "REN021", ApartmentId = "APT050", StartDate = DateTime.Parse("2023-01-01"), EndDate = DateTime.Parse("2023-12-31"), IsConfirmed = true, Price = 15000, Zone = "Soho", PostalCode = "29001" }
            );

            // SEED: CashFlows (ingresos y gastos para edificios, aún más ampliado)
            modelBuilder.Entity<CashFlow>().HasData(
                // BLD001 (Madrid)
                new CashFlow { CashFlowId = "CAF001", BuildingId = "BLD001", Date = DateTime.Parse("2020-01-10"), Amount = 350000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF002", BuildingId = "BLD001", Date = DateTime.Parse("2021-01-10"), Amount = 20000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF003", BuildingId = "BLD001", Date = DateTime.Parse("2021-06-10"), Amount = 5000, Source = "Reforma cocina" },
                new CashFlow { CashFlowId = "CAF004", BuildingId = "BLD001", Date = DateTime.Parse("2022-01-10"), Amount = 21000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF005", BuildingId = "BLD001", Date = DateTime.Parse("2022-06-10"), Amount = 1200, Source = "Seguro edificio" },
                new CashFlow { CashFlowId = "CAF006", BuildingId = "BLD001", Date = DateTime.Parse("2023-01-10"), Amount = 22000, Source = "Alquiler anual" },
                // BLD002 (Barcelona)
                new CashFlow { CashFlowId = "CAF007", BuildingId = "BLD002", Date = DateTime.Parse("2021-05-20"), Amount = 320000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF008", BuildingId = "BLD002", Date = DateTime.Parse("2022-01-10"), Amount = 18000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF009", BuildingId = "BLD002", Date = DateTime.Parse("2022-06-10"), Amount = 3000, Source = "Reforma baño" },
                new CashFlow { CashFlowId = "CAF010", BuildingId = "BLD002", Date = DateTime.Parse("2023-01-10"), Amount = 18500, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF011", BuildingId = "BLD002", Date = DateTime.Parse("2023-03-10"), Amount = 950, Source = "Impuesto IBI" },
                // BLD003 (Sevilla)
                new CashFlow { CashFlowId = "CAF012", BuildingId = "BLD003", Date = DateTime.Parse("2022-03-15"), Amount = 280000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF013", BuildingId = "BLD003", Date = DateTime.Parse("2023-01-10"), Amount = 17000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF014", BuildingId = "BLD003", Date = DateTime.Parse("2023-06-10"), Amount = 2500, Source = "Gasto extraordinario" },
                // BLD004 (Valencia)
                new CashFlow { CashFlowId = "CAF015", BuildingId = "BLD004", Date = DateTime.Parse("2019-07-22"), Amount = 250000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF016", BuildingId = "BLD004", Date = DateTime.Parse("2020-01-10"), Amount = 16000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF017", BuildingId = "BLD004", Date = DateTime.Parse("2020-06-10"), Amount = 1100, Source = "Limpieza fachada" },
                // BLD005 (Bilbao)
                new CashFlow { CashFlowId = "CAF018", BuildingId = "BLD005", Date = DateTime.Parse("2018-11-30"), Amount = 400000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF019", BuildingId = "BLD005", Date = DateTime.Parse("2019-01-10"), Amount = 19000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF020", BuildingId = "BLD005", Date = DateTime.Parse("2019-06-10"), Amount = 4000, Source = "Reforma eléctrica" },
                new CashFlow { CashFlowId = "CAF021", BuildingId = "BLD005", Date = DateTime.Parse("2020-01-10"), Amount = 20000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF022", BuildingId = "BLD005", Date = DateTime.Parse("2020-06-10"), Amount = 950, Source = "Impuesto IBI" },
                // BLD006 (Granada)
                new CashFlow { CashFlowId = "CAF023", BuildingId = "BLD006", Date = DateTime.Parse("2020-06-15"), Amount = 210000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF024", BuildingId = "BLD006", Date = DateTime.Parse("2021-01-10"), Amount = 14000, Source = "Alquiler anual" },
                // BLD007 (Zaragoza)
                new CashFlow { CashFlowId = "CAF025", BuildingId = "BLD007", Date = DateTime.Parse("2021-02-20"), Amount = 90000, Source = "Compra inicial" },
                // BLD008 (Malaga)
                new CashFlow { CashFlowId = "CAF026", BuildingId = "BLD008", Date = DateTime.Parse("2019-09-10"), Amount = 95000, Source = "Compra inicial" },
                // BLD009 (Alicante)
                new CashFlow { CashFlowId = "CAF027", BuildingId = "BLD009", Date = DateTime.Parse("2022-01-05"), Amount = 100000, Source = "Compra inicial" },
                // BLD010 (Pamplona, Estafeta)
                new CashFlow { CashFlowId = "CAF028", BuildingId = "BLD010", Date = DateTime.Parse("2020-03-18"), Amount = 300000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF029", BuildingId = "BLD010", Date = DateTime.Parse("2021-01-10"), Amount = 24000, Source = "Alquiler anual" },
                // BLD011 (Pamplona, Pérez Goyena)
                new CashFlow { CashFlowId = "CAF030", BuildingId = "BLD011", Date = DateTime.Parse("2021-09-01"), Amount = 270000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF031", BuildingId = "BLD011", Date = DateTime.Parse("2022-01-10"), Amount = 22000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF032", BuildingId = "BLD011", Date = DateTime.Parse("2022-06-10"), Amount = 1200, Source = "Seguro edificio" },
                // Añadidos cashflows para los nuevos edificios
                new CashFlow { CashFlowId = "CAF040", BuildingId = "BLD012", Date = DateTime.Parse("2021-03-15"), Amount = 320000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF041", BuildingId = "BLD012", Date = DateTime.Parse("2022-01-10"), Amount = 12000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF042", BuildingId = "BLD012", Date = DateTime.Parse("2022-06-10"), Amount = 2500, Source = "Reforma fachada" },
                new CashFlow { CashFlowId = "CAF050", BuildingId = "BLD013", Date = DateTime.Parse("2022-05-10"), Amount = 410000, Source = "Compra inicial" },
                new CashFlow { CashFlowId = "CAF051", BuildingId = "BLD013", Date = DateTime.Parse("2023-01-10"), Amount = 15000, Source = "Alquiler anual" },
                new CashFlow { CashFlowId = "CAF052", BuildingId = "BLD013", Date = DateTime.Parse("2023-06-10"), Amount = 1800, Source = "Gasto comunidad" }
            );

            // SEED: MaintenanceEvents (eventos de mantenimiento para edificios, aún más ampliado)
            modelBuilder.Entity<MaintenanceEvent>().HasData(
                new MaintenanceEvent { MaintenanceEventId = "MAE001", BuildingId = "BLD001", Description = "Revisión ascensor", Cost = 1000, Date = DateTime.Parse("2023-01-15") },
                new MaintenanceEvent { MaintenanceEventId = "MAE002", BuildingId = "BLD001", Description = "Cambio de caldera", Cost = 1200, Date = DateTime.Parse("2023-06-10") },
                new MaintenanceEvent { MaintenanceEventId = "MAE003", BuildingId = "BLD002", Description = "Pintura fachada", Cost = 900, Date = DateTime.Parse("2023-02-10") },
                new MaintenanceEvent { MaintenanceEventId = "MAE004", BuildingId = "BLD002", Description = "Reparación tejado", Cost = 1100, Date = DateTime.Parse("2023-05-15") },
                new MaintenanceEvent { MaintenanceEventId = "MAE005", BuildingId = "BLD003", Description = "Cambio caldera", Cost = 1200, Date = DateTime.Parse("2023-03-05") },
                new MaintenanceEvent { MaintenanceEventId = "MAE006", BuildingId = "BLD004", Description = "Reparación tejado", Cost = 1100, Date = DateTime.Parse("2023-05-15") },
                new MaintenanceEvent { MaintenanceEventId = "MAE007", BuildingId = "BLD005", Description = "Revisión eléctrica", Cost = 900, Date = DateTime.Parse("2023-06-10") },
                new MaintenanceEvent { MaintenanceEventId = "MAE008", BuildingId = "BLD005", Description = "Cambio caldera", Cost = 1200, Date = DateTime.Parse("2024-01-10") },
                new MaintenanceEvent { MaintenanceEventId = "MAE009", BuildingId = "BLD006", Description = "Pintura fachada", Cost = 800, Date = DateTime.Parse("2023-08-10") },
                new MaintenanceEvent { MaintenanceEventId = "MAE010", BuildingId = "BLD010", Description = "Reparación portal", Cost = 1000, Date = DateTime.Parse("2023-09-01") },
                new MaintenanceEvent { MaintenanceEventId = "MAE011", BuildingId = "BLD011", Description = "Reparación tejado ático", Cost = 900, Date = DateTime.Parse("2023-10-05") },
                // Añadidos eventos de mantenimiento para los nuevos edificios
                new MaintenanceEvent { MaintenanceEventId = "MAE020", BuildingId = "BLD012", Description = "Revisión ascensor", Cost = 950, Date = DateTime.Parse("2022-04-15") },
                new MaintenanceEvent { MaintenanceEventId = "MAE021", BuildingId = "BLD013", Description = "Reparación portal", Cost = 1100, Date = DateTime.Parse("2023-07-01") }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
