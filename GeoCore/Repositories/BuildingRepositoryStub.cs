using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class BuildingRepositoryStub : IBuildingRepository
    {
        private readonly List<Building> _buildings = new()
        {
            new Building { BuildingId = 1, BuildingCode = "BLD001", Name = "Edificio Central", Address = "Calle Mayor 1", City = "Madrid", Latitude = 40.4168, Longitude = -3.7038, PurchaseDate = DateTime.Parse("2020-01-01"), Status = "Active" },
            new Building { BuildingId = 2, BuildingCode = "BLD002", Name = "Edificio Norte", Address = "Avenida Diagonal 211", City = "Barcelona", Latitude = 41.4036, Longitude = 2.1744, PurchaseDate = DateTime.Parse("2021-05-15"), Status = "Rented" },
            new Building { BuildingId = 3, BuildingCode = "BLD003", Name = "Edificio Sur", Address = "Avenida de la Constitución 1", City = "Sevilla", Latitude = 37.3886, Longitude = -5.9957, PurchaseDate = DateTime.Parse("2022-03-10"), Status = "Under Maintenance" },
            new Building { BuildingId = 4, BuildingCode = "BLD004", Name = "Edificio Este", Address = "Calle de la Paz 15", City = "Valencia", Latitude = 39.4702, Longitude = -0.3768, PurchaseDate = DateTime.Parse("2019-07-22"), Status = "Active" },
            new Building { BuildingId = 5, BuildingCode = "BLD005", Name = "Edificio Oeste", Address = "Gran Vía 20", City = "Bilbao", Latitude = 43.2630, Longitude = -2.9350, PurchaseDate = DateTime.Parse("2018-11-30"), Status = "Rented" },
            new Building { BuildingId = 6, BuildingCode = "BLD006", Name = "Edificio Granada", Address = "Calle Reyes Católicos 17", City = "Granada", Latitude = 37.1765, Longitude = -3.5995, PurchaseDate = DateTime.Parse("2020-06-15"), Status = "Active" },
            new Building { BuildingId = 7, BuildingCode = "BLD007", Name = "Edificio Zaragoza", Address = "Paseo Independencia 24", City = "Zaragoza", Latitude = 41.6488, Longitude = -0.8891, PurchaseDate = DateTime.Parse("2021-02-20"), Status = "Under Maintenance" },
            new Building { BuildingId = 8, BuildingCode = "BLD008", Name = "Edificio Málaga", Address = "Calle Larios 4", City = "Malaga", Latitude = 36.7213, Longitude = -4.4214, PurchaseDate = DateTime.Parse("2019-09-10"), Status = "Active" },
            new Building { BuildingId = 9, BuildingCode = "BLD009", Name = "Edificio Alicante", Address = "Avenida Maisonnave 41", City = "Alicante", Latitude = 38.3452, Longitude = -0.4810, PurchaseDate = DateTime.Parse("2022-01-05"), Status = "Rented" },
            new Building { BuildingId = 10, BuildingCode = "BLD010", Name = "Edificio Pamplona", Address = "Calle Estafeta 1", City = "Pamplona", Latitude = 42.8169, Longitude = -1.6446, PurchaseDate = DateTime.Parse("2020-03-18"), Status = "Active" }
        };
        public Task<IEnumerable<Building>> GetAllAsync() => Task.FromResult(_buildings.AsEnumerable());
        public Task<Building?> GetByIdAsync(int id) => Task.FromResult(_buildings.FirstOrDefault(b => b.BuildingId == id));
        public Task<Building?> GetByCodeAsync(string code) => Task.FromResult(_buildings.FirstOrDefault(b => b.BuildingCode == code));
        public Task AddAsync(Building entity)
        {
            // Asignar un BuildingId único
            entity.BuildingId = _buildings.Any() ? _buildings.Max(b => b.BuildingId) + 1 : 1;
            _buildings.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(Building entity) { }
        public void Remove(Building entity) { _buildings.Remove(entity); }
    }
}
