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
    public interface IReceiptDocumentRepository
    {
        Task<IEnumerable<ReceiptDocument>> GetAllAsync(ReceiptDocumentFilter? filter = null, CancellationToken cancellationToken = default);
        Task<ReceiptDocument?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<ReceiptDocument?> GetByNumberAsync(string number, CancellationToken cancellationToken);
        Task<int> CreateAsync(ReceiptDocument document, CancellationToken cancellationToken);
        Task UpdateAsync(ReceiptDocument document, CancellationToken cancellationToken);
        Task DeleteAsync(ReceiptDocument document, CancellationToken cancellationToken);
    }
}
