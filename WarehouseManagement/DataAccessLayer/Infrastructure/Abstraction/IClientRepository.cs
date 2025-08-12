using DataAccessLayer.Models;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default);
        Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Client?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task CreateAsync(Client client, CancellationToken cancellationToken);
        Task UpdateAsync(Client client, CancellationToken cancellationToken);
        Task DeleteAsync(Client client, CancellationToken cancellationToken);
    }
}
