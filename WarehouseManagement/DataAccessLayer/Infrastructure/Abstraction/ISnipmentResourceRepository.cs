using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface ISnipmentResourceRepository
    {
        Task<IEnumerable<SnipmentResource>> GetResourceByResourceId(int resourceId, CancellationToken cancellationToken);
        Task<IEnumerable<SnipmentResource>> GetResourceByUnitId(int unitId, CancellationToken cancellationToken);
        Task<IEnumerable<SnipmentResource>> GetResources(int documentId, CancellationToken cancellationToken);
        Task<SnipmentResource?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateRangeAsync(IEnumerable<SnipmentResource> resources, CancellationToken cancellationToken);
        Task UpdateAsync(SnipmentResource resource, CancellationToken cancellationToken);
        Task DeleteAsync(SnipmentResource resource, CancellationToken cancellationToken);
    }
}
