using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class ApartmentRepositoryStub : IApartmentRepository
    {
        private readonly List<Apartment> _apartments = new()
        {
            new Apartment { ApartmentId = 1, ApartmentCode = "APT001", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 120000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = 1, HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-01") },
            new Apartment { ApartmentId = 2, ApartmentCode = "APT002", ApartmentDoor = "2B", ApartmentFloor = "2", ApartmentPrice = 135000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = 1, HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-02") },
            new Apartment { ApartmentId = 3, ApartmentCode = "APT003", ApartmentDoor = "3C", ApartmentFloor = "3", ApartmentPrice = 110000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = 2, HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-03") },
            new Apartment { ApartmentId = 4, ApartmentCode = "APT004", ApartmentDoor = "1B", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = 3, HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-04") },
            new Apartment { ApartmentId = 5, ApartmentCode = "APT005", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 140000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = 4, HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-05") },
            new Apartment { ApartmentId = 6, ApartmentCode = "APT006", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 115000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = 5, HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-06") },
            new Apartment { ApartmentId = 7, ApartmentCode = "APT007", ApartmentDoor = "1C", ApartmentFloor = "1", ApartmentPrice = 130000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = 6, HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-07") },
            new Apartment { ApartmentId = 8, ApartmentCode = "APT008", ApartmentDoor = "2C", ApartmentFloor = "2", ApartmentPrice = 145000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = 7, HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-08") },
            new Apartment { ApartmentId = 9, ApartmentCode = "APT009", ApartmentDoor = "3B", ApartmentFloor = "3", ApartmentPrice = 112000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = 8, HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-09") },
            new Apartment { ApartmentId = 10, ApartmentCode = "APT010", ApartmentDoor = "1D", ApartmentFloor = "1", ApartmentPrice = 128000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = 9, HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-10") }
        };
        public Task<IEnumerable<Apartment>> GetAllAsync() => Task.FromResult(_apartments.AsEnumerable());
        public Task<Apartment?> GetByIdAsync(int id) => Task.FromResult(_apartments.FirstOrDefault(a => a.ApartmentId == id));
        public Task<IEnumerable<Apartment>> GetByBuildingIdAsync(int buildingId) => Task.FromResult(_apartments.Where(a => a.BuildingId == buildingId).AsEnumerable());
        public Task AddAsync(Apartment entity) { _apartments.Add(entity); return Task.CompletedTask; }
        public void Update(Apartment entity) { }
        public void Remove(Apartment entity) { _apartments.Remove(entity); }
    }
}
