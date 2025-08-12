using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class SnipmentDocumentRepository : ISnipmentDocumentRepository
    {
        private readonly AppDbContext _context;
        public SnipmentDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SnipmentDocument>> GetAllAsync(SnipmentDocumentFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var query = _context.SnipmentDocuments.AsQueryable();
            query = query
                .Include(x => x.Client)
                .Include(x => x.SnipmentResources)
                    .ThenInclude(x => x.Resource)
                .Include(x => x.SnipmentResources)
                    .ThenInclude(x => x.Unit)
                ;

            query = ApplyFilter(query, filter);

            var sql = query.ToQueryString();

            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<SnipmentDocument> ApplyFilter(IQueryable<SnipmentDocument> query, SnipmentDocumentFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.PeriodS != null)
            {
                query = query.Where(x => x.Date >= filter.PeriodS);
            }

            if (filter.PeriodPo != null)
            {
                query = query.Where(x => x.Date <= filter.PeriodPo);
            }

            if (filter.Numbers != null && filter.Numbers.Count() > 0)
            {
                query = query.Where(x => filter.Numbers.Contains(x.Number));
            }

            if (filter.Resources != null && filter.Resources.Count() > 0)
            {
                query = query.Where(x => x.SnipmentResources.Any(y => filter.Resources.Contains(y.ResourceId)));
            }

            if (filter.Units != null && filter.Units.Count() > 0)
            {
                query = query.Where(x => x.SnipmentResources.Any(y => filter.Units.Contains(y.UnitId)));
            }

            if (filter.Clients != null && filter.Clients.Count() > 0)
            {
                query = query.Where(x => filter.Clients.Contains(x.ClientId));
            }

            query = query
              .Select(x => new SnipmentDocument
              {
                  Id = x.Id,
                  Number = x.Number,
                  Date = x.Date,
                  ClientId = x.ClientId,
                  Client = x.Client,
                  Sign = x.Sign,
                  SnipmentResources = x.SnipmentResources
                  .Where(y =>
                        (filter.Units == null || !filter.Units.Any() || filter.Units.Contains(y.UnitId)) &&
                        (filter.Resources == null || !filter.Resources.Any() || filter.Resources.Contains(y.ResourceId)))
                  .ToList()
              });

            return query;
        }

        public async Task<SnipmentDocument?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.SnipmentDocuments
                .Include(x => x.SnipmentResources)
                    .ThenInclude(x => x.Resource)
                .Include(x => x.SnipmentResources)
                    .ThenInclude(x => x.Unit)
                .Include(x => x.Client)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<SnipmentDocument?> GetByNumberAsync(string number, CancellationToken cancellationToken)
        {
            return await _context.SnipmentDocuments.FirstOrDefaultAsync(x => x.Number == number, cancellationToken);
        }

        public async Task<int> CreateAsync(SnipmentDocument document, CancellationToken cancellationToken)
        {
            await _context.SnipmentDocuments.AddAsync(document, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return document.Id;
        }

        public async Task UpdateAsync(SnipmentDocument document, CancellationToken cancellationToken)
        {
            _context.SnipmentDocuments.Update(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(SnipmentDocument document, CancellationToken cancellationToken)
        {
            _context.SnipmentDocuments.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
