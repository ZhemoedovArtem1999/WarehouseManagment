using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface IResourceService
    {
        Task<ResourceDto> GetAllResourceAsync(bool isArchive, CancellationToken cancellationToken);
        Task<ResourceItem> GetResourceAsync(int id, CancellationToken cancellationToken);
        Task CreateResourceAsync(string name, CancellationToken cancellationToken);
        Task EditStateAsync(ResourceUpdateStateRequestDto requestDto, CancellationToken cancellationToken);
        Task UpdateResourceAsync(ResourceUpdateRequestDto requestDto, CancellationToken cancellationToken);
        Task DeleteResource(int id, CancellationToken cancellationToken);
    }
}
