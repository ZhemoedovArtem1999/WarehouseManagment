using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Infrastructure.Repository;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.Balance;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface IBalanceService
    {
        Task<BalanceFilterData> GetFilterData(CancellationToken cancellationToken);
        Task<IEnumerable<BalanceDto>> GetBalance(BalanceFilter filter, CancellationToken cancellationToken);
        Task UpdateBalanceAsync(int resourceId, int unitId, decimal countResource, CancellationToken cancellationToken);
    }
}
