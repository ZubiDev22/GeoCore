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
            // BLD001
            new Apartment { ApartmentId = "APT001", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 120000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD001", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-01"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT002", ApartmentDoor = "1B", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD001", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-02"), Status = "libre" },
            new Apartment { ApartmentId = "APT003", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 130000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD001", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-03"), Status = "reservado" },
            // BLD002
            new Apartment { ApartmentId = "APT004", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 110000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD002", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-04"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT005", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 115000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD002", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-05"), Status = "libre" },
            new Apartment { ApartmentId = "APT006", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 118000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD002", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-06"), Status = "reservado" },
            // BLD003
            new Apartment { ApartmentId = "APT007", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 140000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD003", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-07"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT008", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 145000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD003", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-08"), Status = "libre" },
            new Apartment { ApartmentId = "APT009", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 150000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD003", HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-09"), Status = "reservado" },
            // BLD004
            new Apartment { ApartmentId = "APT010", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 112000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD004", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-10"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT011", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 117000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD004", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-11"), Status = "libre" },
            new Apartment { ApartmentId = "APT012", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 119000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD004", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-12"), Status = "reservado" },
            // BLD005
            new Apartment { ApartmentId = "APT013", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD005", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-13"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT014", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 130000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD005", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-14"), Status = "libre" },
            new Apartment { ApartmentId = "APT015", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 135000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD005", HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-15"), Status = "reservado" },
            // BLD006
            new Apartment { ApartmentId = "APT016", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 128000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD006", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-16"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT017", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 133000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD006", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-17"), Status = "libre" },
            new Apartment { ApartmentId = "APT018", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 138000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD006", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-18"), Status = "reservado" },
            // BLD007
            new Apartment { ApartmentId = "APT019", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 140000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD007", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-19"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT020", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 145000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD007", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-20"), Status = "libre" },
            new Apartment { ApartmentId = "APT021", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 150000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD007", HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-21"), Status = "reservado" },
            // BLD008
            new Apartment { ApartmentId = "APT022", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 112000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD008", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-22"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT023", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 117000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD008", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-23"), Status = "libre" },
            new Apartment { ApartmentId = "APT024", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 119000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD008", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-24"), Status = "reservado" },
            // BLD009
            new Apartment { ApartmentId = "APT025", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 125000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD009", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-25"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT026", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 130000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD009", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-26"), Status = "libre" },
            new Apartment { ApartmentId = "APT027", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 135000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD009", HasLift = false, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-27"), Status = "reservado" },
            // BLD010
            new Apartment { ApartmentId = "APT028", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 128000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD010", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-28"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT029", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 133000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD010", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-01-29"), Status = "libre" },
            new Apartment { ApartmentId = "APT030", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 138000, NumberOfRooms = 2, NumberOfBathrooms = 1, BuildingId = "BLD010", HasLift = false, HasGarage = false, CreatedDate = DateTime.Parse("2023-01-30"), Status = "reservado" },
            // BLD011 (Pérez Goyena 28, Pamplona)
            new Apartment { ApartmentId = "APT031", ApartmentDoor = "1A", ApartmentFloor = "1", ApartmentPrice = 150000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-02-01"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT032", ApartmentDoor = "2A", ApartmentFloor = "2", ApartmentPrice = 155000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-02-02"), Status = "libre" },
            new Apartment { ApartmentId = "APT033", ApartmentDoor = "3A", ApartmentFloor = "3", ApartmentPrice = 160000, NumberOfRooms = 4, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-02-03"), Status = "reservado" },
            new Apartment { ApartmentId = "APT034", ApartmentDoor = "4A", ApartmentFloor = "4", ApartmentPrice = 165000, NumberOfRooms = 3, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = false, CreatedDate = DateTime.Parse("2023-02-04"), Status = "ocupado" },
            new Apartment { ApartmentId = "APT035", ApartmentDoor = "Atico", ApartmentFloor = "5", ApartmentPrice = 180000, NumberOfRooms = 2, NumberOfBathrooms = 2, BuildingId = "BLD011", HasLift = true, HasGarage = true, CreatedDate = DateTime.Parse("2023-02-05"), Status = "libre" }
        };
        public Task<IEnumerable<Apartment>> GetAllAsync() => Task.FromResult(_apartments.AsEnumerable());
        public Task<Apartment?> GetByIdAsync(string id) => Task.FromResult(_apartments.FirstOrDefault(a => a.ApartmentId == id));
        public Task<IEnumerable<Apartment>> GetByBuildingIdAsync(string buildingId) => Task.FromResult(_apartments.Where(a => a.BuildingId == buildingId).AsEnumerable());
        public Task AddAsync(Apartment entity)
        {
            int nextNum = 1;
            if (_apartments.Any())
            {
                var last = _apartments
                    .Select(a => a.ApartmentId)
                    .Where(id => id.StartsWith("APT"))
                    .Select(id => int.TryParse(id.Substring(3), out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                nextNum = last + 1;
            }
            entity.ApartmentId = $"APT{nextNum.ToString("D3")}";
            _apartments.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(Apartment entity) { }
        public void Remove(Apartment entity) { _apartments.Remove(entity); }
    }
}
