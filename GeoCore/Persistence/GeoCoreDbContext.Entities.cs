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

            // SEED: Buildings
            modelBuilder.Entity<Building>().HasData(
                new Building {
                    BuildingId = "BLD002",
                    BuildingCode = "BLD002",
                    Name = "Edificio Diagonal",
                    Address = "Carrer de Sardenya 350",
                    City = "Barcelona",
                    Latitude = 41.40432,
                    Longitude = 2.17403,
                    PurchaseDate = new DateTime(2021, 5, 15),
                    Status = "Rented",
                    PostalCode = "08025"
                }
            );

            // SEED: Apartments (deben pertenecer a BLD002)
            modelBuilder.Entity<Apartment>().HasData(
                new Apartment {
                    ApartmentId = "APT001",
                    BuildingId = "BLD002",
                    ApartmentDoor = "1A",
                    ApartmentFloor = "1",
                    ApartmentPrice = 1000,
                    NumberOfRooms = 2,
                    NumberOfBathrooms = 1,
                    HasLift = true,
                    HasGarage = false,
                    CreatedDate = new DateTime(2021, 5, 15),
                    Status = "Rented"
                },
                new Apartment {
                    ApartmentId = "APT002",
                    BuildingId = "BLD002",
                    ApartmentDoor = "2A",
                    ApartmentFloor = "2",
                    ApartmentPrice = 1200,
                    NumberOfRooms = 3,
                    NumberOfBathrooms = 2,
                    HasLift = true,
                    HasGarage = true,
                    CreatedDate = new DateTime(2021, 5, 15),
                    Status = "Rented"
                }
            );

            // SEED: Rentals (asociados a los apartamentos de BLD002)
            modelBuilder.Entity<Rental>().HasData(
                new Rental {
                    RentalId = "REN001",
                    ApartmentId = "APT001",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    IsConfirmed = true,
                    Price = 1200,
                    Zone = "Eixample",
                    PostalCode = "08025"
                },
                new Rental {
                    RentalId = "REN002",
                    ApartmentId = "APT002",
                    StartDate = new DateTime(2024, 2, 1),
                    EndDate = new DateTime(2024, 12, 31),
                    IsConfirmed = true,
                    Price = 1300,
                    Zone = "Eixample",
                    PostalCode = "08025"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
