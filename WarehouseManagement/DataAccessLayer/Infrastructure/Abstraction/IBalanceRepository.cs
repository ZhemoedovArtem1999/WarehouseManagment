using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Models;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface IBalanceRepository
    {
        Task<IEnumerable<Balance>> GetAllAsync(BalanceFilter? filter = null, CancellationToken cancellationToken = default);
        Task<Balance> GetBalanceWithLockAsync(int resourceId, int unitId, CancellationToken cancellationToken);
        Task CreateAsync(Balance balance, CancellationToken cancellationToken);
        Task UpdateAsync(Balance balance, CancellationToken cancellationToken);
        Task DeleteAsync(Balance balance, CancellationToken cancellationToken);
    }
}
