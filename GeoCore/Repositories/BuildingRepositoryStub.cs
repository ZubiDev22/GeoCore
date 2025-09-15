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
            // Coordenadas verificadas con Nominatim/OpenStreetMap
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
            new Building { BuildingId = "BLD011", BuildingCode = "BLD011", Name = "Edificio Pérez Goyena", Address = "Calle Pérez Goyena 28", City = "Pamplona", Latitude = 42.800900, Longitude = -1.616800, PurchaseDate = DateTime.Parse("2023-01-01"), Status = "Active", PostalCode = "31620" }
        };
        public Task<IEnumerable<Building>> GetAllAsync() => Task.FromResult(_buildings.AsEnumerable());
        public Task<Building?> GetByIdAsync(string id) => Task.FromResult(_buildings.FirstOrDefault(b => b.BuildingId == id));
        public Task<Building?> GetByCodeAsync(string code) => Task.FromResult(_buildings.FirstOrDefault(b => b.BuildingCode == code));
        public Task AddAsync(Building entity)
        {
            int nextNum = 1;
            if (_buildings.Any())
            {
                var last = _buildings
                    .Select(b => b.BuildingId)
                    .Where(id => id.StartsWith("BLD"))
                    .Select(id => int.TryParse(id.Substring(3), out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                nextNum = last + 1;
            }
            entity.BuildingId = $"BLD{nextNum.ToString("D3")}";
            _buildings.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(Building entity) { }
        public void Remove(Building entity) { _buildings.Remove(entity); }

        // Ejemplo LINQ 1: Filtrar edificios en una ciudad y cuyo nombre contiene una letra, ordenados por nombre
        public IEnumerable<Building> GetByCityAndNameContains(string city, string letter)
        {
            return from building in _buildings
                   where building.City == city && building.Name.Contains(letter)
                   orderby building.Name
                   select building;
        }

        // Ejemplo LINQ 2: Filtrar edificios por estado
        public IEnumerable<Building> GetByStatus(string status)
        {
            return _buildings.Where(b => b.Status == status);
        }

        // Ejemplo LINQ 3: Obtener edificios ordenados por nombre
        public IEnumerable<Building> GetOrderedByName()
        {
            return _buildings.OrderBy(b => b.Name);
        }

        // Ejemplo LINQ 4: Seleccionar nombres de edificios con longitud mayor a 6
        public IEnumerable<string> GetLongBuildingNames()
        {
            return _buildings.Where(b => b.Name.Length > 6).OrderBy(b => b.Name).Select(b => b.Name);
        }
    }
}
