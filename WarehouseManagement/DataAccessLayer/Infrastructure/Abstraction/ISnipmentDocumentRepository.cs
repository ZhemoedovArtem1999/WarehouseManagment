using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface ISnipmentDocumentRepository
    {
        Task<IEnumerable<SnipmentDocument>> GetAllAsync(SnipmentDocumentFilter? filter = null, CancellationToken cancellationToken = default);
        Task<SnipmentDocument?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<SnipmentDocument?> GetByNumberAsync(string number, CancellationToken cancellationToken);
        Task<int> CreateAsync(SnipmentDocument document, CancellationToken cancellationToken);
        Task UpdateAsync(SnipmentDocument document, CancellationToken cancellationToken);
        Task DeleteAsync(SnipmentDocument document, CancellationToken cancellationToken);
    }
}
