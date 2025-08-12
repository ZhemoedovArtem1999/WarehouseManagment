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
    public class BalanceRepository : IBalanceRepository
    {
        private readonly AppDbContext _context;
        public BalanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Balance>> GetAllAsync(BalanceFilter? filter = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Balances.AsQueryable();

            query = query
                .Include(x => x.Resource)
                .Include(x => x.Unit);

            query = ApplyFilter(query, filter);

            return await query.ToListAsync(cancellationToken);
        }

        private IQueryable<Balance> ApplyFilter(IQueryable<Balance> query, BalanceFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.Resources != null && filter.Resources.Count() > 0)
            {
                query = query.Where(x => filter.Resources.Contains(x.ResourceId));
            }

            if (filter.Units != null && filter.Units.Count() > 0)
            {
                query = query.Where(x => filter.Units.Contains(x.UnitId));
            }

            return query;
        }

        public async Task<Balance> GetBalanceWithLockAsync(int resourceId, int unitId, CancellationToken cancellationToken)
        {
            var balance = await _context.Balances
                .FromSqlInterpolated(
                    $@"SELECT * FROM ""warehouse"".""balance"" 
                        WHERE ""resource_id"" = {resourceId} AND ""unit_id"" = {unitId} 
                        FOR UPDATE"
                )
                .FirstOrDefaultAsync(cancellationToken);

            return balance;
        }

        public async Task CreateAsync(Balance balance, CancellationToken cancellationToken)
        {
            await _context.Balances.AddAsync(balance, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Balance balance, CancellationToken cancellationToken)
        {
            _context.Balances.Update(balance);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Balance balance, CancellationToken cancellationToken)
        {
            _context.Balances.Remove(balance);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
