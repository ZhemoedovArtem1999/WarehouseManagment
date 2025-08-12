using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Repository
{
    public class ReceiptDocumentRepository : IReceiptDocumentRepository
    {
        private readonly AppDbContext _context;
        public ReceiptDocumentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReceiptDocument>> GetAllAsync(ReceiptDocumentFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var query = _context.ReceiptDocuments.AsQueryable();

            query = query
                .Include(x => x.ReceiptResources)
                    .ThenInclude(x => x.Resource)
                .Include(x => x.ReceiptResources)
                    .ThenInclude(x => x.Unit);

            query = ApplyFilter(query, filter);

            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<ReceiptDocument> ApplyFilter(IQueryable<ReceiptDocument> query, ReceiptDocumentFilter filter)
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
                query = query.Where(x => x.ReceiptResources.Any(y => filter.Resources.Contains(y.ResourceId)));
            }

            if (filter.Units != null && filter.Units.Count() > 0)
            {
                query = query.Where(x => x.ReceiptResources.Any(y => filter.Units.Contains(y.UnitId)));
            }

            query = query
                .Select(x => new ReceiptDocument
                {
                    Id = x.Id,
                    Number = x.Number,
                    Date = x.Date,
                    ReceiptResources = x.ReceiptResources
                    .Where(y =>
                          (filter.Units == null || !filter.Units.Any() || filter.Units.Contains(y.UnitId)) &&
                          (filter.Resources == null || !filter.Resources.Any() || filter.Resources.Contains(y.ResourceId)))
                    .ToList()
                }
            );

            return query;
        }

        public async Task<ReceiptDocument?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.ReceiptDocuments
                .Include(x => x.ReceiptResources)
                    .ThenInclude(x => x.Resource)
                .Include(x => x.ReceiptResources)
                    .ThenInclude(x => x.Unit)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<ReceiptDocument?> GetByNumberAsync(string number, CancellationToken cancellationToken)
        {
            return await _context.ReceiptDocuments.FirstOrDefaultAsync(x => x.Number == number, cancellationToken);
        }

        public async Task<int> CreateAsync(ReceiptDocument document, CancellationToken cancellationToken)
        {
            await _context.ReceiptDocuments.AddAsync(document, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return document.Id;
        }

        public async Task UpdateAsync(ReceiptDocument document, CancellationToken cancellationToken)
        {
            _context.ReceiptDocuments.Update(document);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(ReceiptDocument document, CancellationToken cancellationToken)
        {
            _context.ReceiptDocuments.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
