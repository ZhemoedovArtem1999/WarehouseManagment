using WarehouseManagementApi.Models.Unit;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface IUnitService
    {
        Task<UnitDto> GetAllUnitAsync(bool isArchive, CancellationToken cancellationToken);
        Task<UnitItem> GetUnitAsync(int id, CancellationToken cancellationToken);
        Task CreateUnitAsync(string name, CancellationToken cancellationToken);
        Task EditStateAsync(UnitUpdateStateRequestDto requestDto, CancellationToken cancellationToken);
        Task UpdateUnitAsync(UnitUpdateRequestDto requestDto, CancellationToken cancellationToken);
        Task DeleteUnitAsync(int id, CancellationToken cancellationToken);
    }
}
