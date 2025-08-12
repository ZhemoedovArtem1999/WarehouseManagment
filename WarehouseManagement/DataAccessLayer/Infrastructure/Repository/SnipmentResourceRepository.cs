using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class SnipmentResourceRepository : ISnipmentResourceRepository
    {
        private readonly AppDbContext _context;
        public SnipmentResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SnipmentResource>> GetResourceByResourceId(int resourceId, CancellationToken cancellationToken)
        {
            return await _context.SnipmentResources.Where(x => x.ResourceId == resourceId).ToListAsync();
        }

        public async Task<IEnumerable<SnipmentResource>> GetResourceByUnitId(int unitId, CancellationToken cancellationToken)
        {
            return await _context.SnipmentResources.Where(x => x.UnitId == unitId).ToListAsync();
        }

        public async Task<IEnumerable<SnipmentResource>> GetResources(int documentId, CancellationToken cancellationToken)
        {
            return await _context.SnipmentResources.Where(x => x.DocumentId == documentId).ToListAsync();
        }

        public async Task<SnipmentResource?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.SnipmentResources.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task CreateRangeAsync(IEnumerable<SnipmentResource> resources, CancellationToken cancellationToken)
        {
            await _context.SnipmentResources.AddRangeAsync(resources, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(SnipmentResource resource, CancellationToken cancellationToken)
        {
            _context.SnipmentResources.Update(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(SnipmentResource resource, CancellationToken cancellationToken)
        {
            _context.SnipmentResources.Remove(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
