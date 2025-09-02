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
            new Building { BuildingCode = "BLD001", Name = "Edificio Central", Address = "Calle Mayor 1", City = "Madrid", Latitude = 40.4168, Longitude = -3.7038, PurchaseDate = DateTime.Parse("2020-01-01"), Status = "Active" },
            new Building { BuildingCode = "BLD002", Name = "Edificio Norte", Address = "Avenida Diagonal 211", City = "Barcelona", Latitude = 41.4036, Longitude = 2.1744, PurchaseDate = DateTime.Parse("2021-05-15"), Status = "Rented" },
            new Building { BuildingCode = "BLD003", Name = "Edificio Sur", Address = "Avenida de la Constitución 1", City = "Sevilla", Latitude = 37.3886, Longitude = -5.9957, PurchaseDate = DateTime.Parse("2022-03-10"), Status = "Under Maintenance" },
            new Building { BuildingCode = "BLD004", Name = "Edificio Este", Address = "Calle de la Paz 15", City = "Valencia", Latitude = 39.4702, Longitude = -0.3768, PurchaseDate = DateTime.Parse("2019-07-22"), Status = "Active" },
            new Building { BuildingCode = "BLD005", Name = "Edificio Oeste", Address = "Gran Vía 20", City = "Bilbao", Latitude = 43.2630, Longitude = -2.9350, PurchaseDate = DateTime.Parse("2018-11-30"), Status = "Rented" },
            new Building { BuildingCode = "BLD006", Name = "Edificio Granada", Address = "Calle Reyes Católicos 17", City = "Granada", Latitude = 37.1765, Longitude = -3.5995, PurchaseDate = DateTime.Parse("2020-06-15"), Status = "Active" },
            new Building { BuildingCode = "BLD007", Name = "Edificio Zaragoza", Address = "Paseo Independencia 24", City = "Zaragoza", Latitude = 41.6488, Longitude = -0.8891, PurchaseDate = DateTime.Parse("2021-02-20"), Status = "Under Maintenance" },
            new Building { BuildingCode = "BLD008", Name = "Edificio Málaga", Address = "Calle Larios 4", City = "Malaga", Latitude = 36.7213, Longitude = -4.4214, PurchaseDate = DateTime.Parse("2019-09-10"), Status = "Active" },
            new Building { BuildingCode = "BLD009", Name = "Edificio Alicante", Address = "Avenida Maisonnave 41", City = "Alicante", Latitude = 38.3452, Longitude = -0.4810, PurchaseDate = DateTime.Parse("2022-01-05"), Status = "Rented" },
            new Building { BuildingCode = "BLD010", Name = "Edificio Córdoba", Address = "Avenida del Gran Capitán 15", City = "Cordoba", Latitude = 37.8882, Longitude = -4.7794, PurchaseDate = DateTime.Parse("2020-03-18"), Status = "Active" },
            new Building { BuildingCode = "BLD011", Name = "Edificio Valladolid", Address = "Plaza Mayor 1", City = "Valladolid", Latitude = 41.6523, Longitude = -4.7245, PurchaseDate = DateTime.Parse("2021-08-12"), Status = "Rented" },
            new Building { BuildingCode = "BLD012", Name = "Edificio Santander", Address = "Calle Juan de Herrera 2", City = "Santander", Latitude = 43.4623, Longitude = -3.8099, PurchaseDate = DateTime.Parse("2018-05-25"), Status = "Active" },
            new Building { BuildingCode = "BLD013", Name = "Edificio Salamanca", Address = "Calle Toro 25", City = "Salamanca", Latitude = 40.9701, Longitude = -5.6635, PurchaseDate = DateTime.Parse("2019-12-01"), Status = "Under Maintenance" },
            new Building { BuildingCode = "BLD014", Name = "Edificio Toledo", Address = "Calle Comercio 12", City = "Toledo", Latitude = 39.8628, Longitude = -4.0273, PurchaseDate = DateTime.Parse("2020-10-10"), Status = "Active" },
            new Building { BuildingCode = "BLD015", Name = "Edificio Burgos", Address = "Avenida del Cid 2", City = "Burgos", Latitude = 42.3439, Longitude = -3.6969, PurchaseDate = DateTime.Parse("2021-04-17"), Status = "Rented" },
            new Building { BuildingCode = "BLD016", Name = "Edificio León", Address = "Calle Ordoño II 10", City = "Leon", Latitude = 42.5987, Longitude = -5.5671, PurchaseDate = DateTime.Parse("2019-03-22"), Status = "Active" },
            new Building { BuildingCode = "BLD017", Name = "Edificio Oviedo", Address = "Calle Uría 18", City = "Oviedo", Latitude = 43.3619, Longitude = -5.8494, PurchaseDate = DateTime.Parse("2022-07-30"), Status = "Under Maintenance" },
            new Building { BuildingCode = "BLD018", Name = "Edificio Logroño", Address = "Calle Portales 30", City = "Logrono", Latitude = 42.4667, Longitude = -2.4456, PurchaseDate = DateTime.Parse("2020-12-05"), Status = "Active" },
            new Building { BuildingCode = "BLD019", Name = "Edificio Huesca", Address = "Calle Coso Bajo 11", City = "Huesca", Latitude = 42.1362, Longitude = -0.4089, PurchaseDate = DateTime.Parse("2021-09-14"), Status = "Rented" },
            new Building { BuildingCode = "BLD020", Name = "Edificio Jaén", Address = "Paseo de la Estación 32", City = "Jaen", Latitude = 37.7796, Longitude = -3.7849, PurchaseDate = DateTime.Parse("2018-08-08"), Status = "Active" },
            new Building { BuildingCode = "BLD021", Name = "Edificio Palencia", Address = "Calle Mayor Principal 20", City = "Palencia", Latitude = 42.0095, Longitude = -4.5241, PurchaseDate = DateTime.Parse("2019-06-19"), Status = "Under Maintenance" },
            new Building { BuildingCode = "BLD022", Name = "Edificio Segovia", Address = "Calle Real 45", City = "Segovia", Latitude = 40.9429, Longitude = -4.1088, PurchaseDate = DateTime.Parse("2022-02-28"), Status = "Active" },
            new Building { BuildingCode = "BLD023", Name = "Edificio Tracasa", Address = "Calle María Victoria, 46", City = "Pamplona", Latitude = 42.7972, Longitude = -1.6341, PurchaseDate = DateTime.MinValue, Status = "Referencia" }
        };
        public Task<IEnumerable<Building>> GetAllAsync() => Task.FromResult(_buildings.AsEnumerable());
        public Task<Building?> GetByIdAsync(int id) => Task.FromResult(_buildings.FirstOrDefault(b => b.BuildingCode == $"BLD{id:D3}"));
        public Task AddAsync(Building entity) { _buildings.Add(entity); return Task.CompletedTask; }
        public void Update(Building entity) { }
        public void Remove(Building entity) { _buildings.Remove(entity); }
    }
}
