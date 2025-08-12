using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.FilterModel;
using System.Data;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.Balance;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceRepository balanceRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly IUnitRepository unitRepository;
        public BalanceService(IBalanceRepository balanceRepository, IResourceRepository resourceRepository, IUnitRepository unitRepository)
        {
            this.balanceRepository = balanceRepository;
            this.resourceRepository = resourceRepository;
            this.unitRepository = unitRepository;
        }

        public async Task<BalanceFilterData> GetFilterData(CancellationToken cancellationToken)
        {
            var units = await unitRepository.GetAllAsync(cancellationToken: cancellationToken);
            var resources = await resourceRepository.GetAllAsync(cancellationToken: cancellationToken);

            var result = new BalanceFilterData
            {
                Units = units.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
                Resources = resources.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
            };

            return result;
        }

        public async Task<IEnumerable<BalanceDto>> GetBalance(BalanceFilter filter, CancellationToken cancellationToken)
        {
            var balances = await balanceRepository.GetAllAsync(filter, cancellationToken);

            var result = balances.Select(x => new BalanceDto
            {
                ResourceId = x.ResourceId,
                ResourceName = x.Resource.Name,
                UnitId = x.UnitId,
                UnitName = x.Unit.Name,
                Count = x.Count
            });

            return result;
        }

        public async Task UpdateBalanceAsync(int resourceId, int unitId, decimal countResource, CancellationToken cancellationToken)
        {
            try
            {
                var balance = await balanceRepository.GetBalanceWithLockAsync(resourceId, unitId, cancellationToken);

                if (balance == null && countResource > 0)
                {
                    throw new InvalidOperationException();
                }
                else if (balance == null)
                {
                    await balanceRepository.CreateAsync(new DataAccessLayer.Models.Balance { ResourceId = resourceId, UnitId = unitId, Count = countResource * (-1) }, cancellationToken);
                }
                else
                {
                    if (balance.Count >= countResource)
                    {
                        balance.Count -= countResource;
                        if (balance.Count == 0)
                        {
                            await balanceRepository.DeleteAsync(balance, cancellationToken);
                        }
                        else
                        {
                            await balanceRepository.UpdateAsync(balance, cancellationToken);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            }catch(InvalidOperationException ex)
            {
                var res = await resourceRepository.GetByIdAsync(resourceId, cancellationToken);
                var unit = await unitRepository.GetByIdAsync(unitId, cancellationToken);
                throw new InvalidOperationException($"На балансе не хватает ресурса {res.Name} с единицей измерения {unit.Name}.");
            }

        }
    }
}
