using DataAccessLayer.Models;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface IUnitRepository
    {
        Task<IEnumerable<UnitMeasurement>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default);
        Task<UnitMeasurement?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<UnitMeasurement?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task CreateAsync(UnitMeasurement unit, CancellationToken cancellationToken);
        Task UpdateAsync(UnitMeasurement unit, CancellationToken cancellationToken);
        Task DeleteAsync(UnitMeasurement unit, CancellationToken cancellationToken);
    }
}
