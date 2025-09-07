using GeoCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace GeoCore.Repositories
{
    public class RentalRepositoryStub : IRentalRepository
    {
        private readonly List<Rental> _rentals = new()
        {
            new Rental { RentalId = "REN001", ApartmentId = "APT001", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 12, 31), IsConfirmed = true, Price = 1200, Zone = "Centro", PostalCode = "28001" },
            new Rental { RentalId = "REN002", ApartmentId = "APT002", StartDate = new DateTime(2023, 2, 1), EndDate = new DateTime(2023, 8, 31), IsConfirmed = true, Price = 1100, Zone = "Eixample", PostalCode = "08009" },
            new Rental { RentalId = "REN003", ApartmentId = "APT003", StartDate = new DateTime(2023, 3, 1), EndDate = new DateTime(2023, 9, 30), IsConfirmed = false, Price = 900, Zone = "Ruzafa", PostalCode = "46004" }
        };
        public Task<IEnumerable<Rental>> GetAllAsync() => Task.FromResult(_rentals.AsEnumerable());
        public Task<Rental?> GetByIdAsync(string id) => Task.FromResult(_rentals.FirstOrDefault(r => r.RentalId == id));
        public Task AddAsync(Rental entity)
        {
            // Generar nuevo RentalId tipo REN001
            int nextNum = 1;
            if (_rentals.Any())
            {
                var last = _rentals
                    .Select(r => r.RentalId)
                    .Where(id => id.StartsWith("REN"))
                    .Select(id => int.TryParse(id.Substring(3), out var n) ? n : 0)
                    .DefaultIfEmpty(0)
                    .Max();
                nextNum = last + 1;
            }
            entity.RentalId = $"REN{nextNum.ToString("D3")}";
            _rentals.Add(entity);
            return Task.CompletedTask;
        }
        public void Update(Rental entity) { }
        public void Remove(Rental entity) { _rentals.Remove(entity); }
    }
}
