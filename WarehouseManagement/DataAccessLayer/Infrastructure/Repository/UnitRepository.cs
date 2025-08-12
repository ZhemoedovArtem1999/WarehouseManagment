using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext _context;
        public UnitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnitMeasurement>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default)
        {
            var query = _context.UnitMeasurements.AsQueryable();

            if (ids != null && ids.Any())
            {
                var idsQuery = query.Where(x => ids.Contains(x.Id));

                var otherQuery = isArchive.HasValue
                    ? query.Where(x => !ids.Contains(x.Id) && x.Archive == isArchive.Value)
                    : query.Where(x => !ids.Contains(x.Id));

                return await idsQuery
                    .Concat(otherQuery)
                    .OrderBy(x => x.Name)
                    .ToListAsync(cancellationToken);
            }

            if (isArchive.HasValue)
            {
                query = query.Where(x => x.Archive == isArchive.Value);
            }

            return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
        }

        public async Task<UnitMeasurement?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.UnitMeasurements.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<UnitMeasurement?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.UnitMeasurements.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task CreateAsync(UnitMeasurement unit, CancellationToken cancellationToken)
        {
            await _context.UnitMeasurements.AddAsync(unit, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UnitMeasurement unit, CancellationToken cancellationToken)
        {
            _context.UnitMeasurements.Update(unit);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(UnitMeasurement unit, CancellationToken cancellationToken)
        {
            _context.UnitMeasurements.Remove(unit);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
