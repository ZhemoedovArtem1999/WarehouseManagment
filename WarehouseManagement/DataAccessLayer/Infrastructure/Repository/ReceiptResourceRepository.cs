using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class ReceiptResourceRepository : IReceiptResourceRepository
    {
        private readonly AppDbContext _context;
        public ReceiptResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceiptResource>> GetResourceByResourceId(int resourceId, CancellationToken cancellationToken)
        {
            return await _context.ReceiptResources.Where(x => x.ResourceId == resourceId).ToListAsync();
        }

        public async Task<IEnumerable<ReceiptResource>> GetResourceByUnitId(int unitId, CancellationToken cancellationToken)
        {
            return await _context.ReceiptResources.Where(x => x.UnitId == unitId).ToListAsync();
        }

        public async Task<IEnumerable<ReceiptResource>> GetResources(int documentId, CancellationToken cancellationToken)
        {
            return await _context.ReceiptResources.Where(x => x.DocumentId == documentId).ToListAsync();
        }

        public async Task<ReceiptResource?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ReceiptResources.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task CreateRangeAsync(IEnumerable<ReceiptResource> resources, CancellationToken cancellationToken)
        {
            await _context.ReceiptResources.AddRangeAsync(resources, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(ReceiptResource resource, CancellationToken cancellationToken)
        {
            _context.ReceiptResources.Update(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(ReceiptResource resource, CancellationToken cancellationToken)
        {
            _context.ReceiptResources.Remove(resource);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
