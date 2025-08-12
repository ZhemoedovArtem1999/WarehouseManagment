using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface IReceiptResourceRepository
    {
        Task<IEnumerable<ReceiptResource>> GetResourceByResourceId(int resourceId, CancellationToken cancellationToken);
        Task<IEnumerable<ReceiptResource>> GetResourceByUnitId(int unitId, CancellationToken cancellationToken);
        Task<IEnumerable<ReceiptResource>> GetResources(int documentId, CancellationToken cancellationToken);
        Task<ReceiptResource?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task CreateRangeAsync(IEnumerable<ReceiptResource> resources, CancellationToken cancellationToken);
        Task UpdateAsync(ReceiptResource resource, CancellationToken cancellationToken);
        Task DeleteAsync(ReceiptResource resource, CancellationToken cancellationToken);
    }
}
