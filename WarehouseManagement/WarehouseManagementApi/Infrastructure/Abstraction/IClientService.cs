using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface IClientService
    {
        Task<ClientDto> GetAllClientAsync(bool isArchive, CancellationToken cancellationToken);
        Task<ClientItem> GetClientAsync(int id, CancellationToken cancellationToken);
        Task CreateClientAsync(ClientInsertRequestDto requestDto, CancellationToken cancellationToken);
        Task EditStateAsync(ClientUpdateStateRequestDto requestDto, CancellationToken cancellationToken);
        Task UpdateClientAsync(ClientUpdateRequestDto requestDto, CancellationToken cancellationToken);
        Task DeleteClientAsync(int id, CancellationToken cancellationToken);
    }
}
