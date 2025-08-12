using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Clients.AsQueryable();

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

        public async Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Client?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Clients.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task CreateAsync(Client client, CancellationToken cancellationToken)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Client client, CancellationToken cancellationToken)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Client client, CancellationToken cancellationToken)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
