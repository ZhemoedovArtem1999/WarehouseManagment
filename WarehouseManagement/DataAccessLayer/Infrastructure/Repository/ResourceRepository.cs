using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext _context;
        public ResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resource>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Resources.AsQueryable();

            if (ids != null && ids.Any())
            {
                var idsQuery = query.Where(x => ids.Contains(x.Id));

                var otherQuery = isArchive.HasValue
                    ? query.Where(x => !ids.Contains(x.Id) && x.Archive == isArchive.Value)
                    : query.Where(x => !ids.Contains(x.Id));

                return await idsQuery
                    .Concat(otherQuery)
                    .OrderBy(x=> x.Name)
                    .ToListAsync(cancellationToken);
            }

            if (isArchive.HasValue)
            {
                query = query.Where(x => x.Archive == isArchive.Value);
            }


            return await query.OrderBy(x => x.Name).ToListAsync(cancellationToken);
        }

        public async Task<Resource?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Resources.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Resource?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Resources.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task CreateAsync(Resource resource, CancellationToken cancellationToken)
        {
            await _context.Resources.AddAsync(resource, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Resource resource, CancellationToken cancellationToken)
        {
            _context.Resources.Update(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Resource resource, CancellationToken cancellationToken)
        {
            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
