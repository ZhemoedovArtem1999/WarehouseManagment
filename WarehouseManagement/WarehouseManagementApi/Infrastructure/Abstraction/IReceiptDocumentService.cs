using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using WarehouseManagementApi.Infrastructure.Services;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.ReceiptDocument;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface IReceiptDocumentService
    {
        Task<ReceiptFilterData> GetFilterData(CancellationToken cancellationToken);
        Task<IEnumerable<ReceiptDocumentViewDto>> GetDocuments(ReceiptDocumentFilter filter, CancellationToken cancellationToken);
        Task<ReceiptResourceReferences> GetReferences(int? documentId = null, CancellationToken cancellationToken = default);
        Task<ReceiptDocumentDto> GetDocument(int id, CancellationToken cancellationToken);
        Task CreateDocumentAsync(ReceiptDocumentDto documentDto, CancellationToken cancellationToken);
        Task UpdateDocumentAsync(ReceiptDocumentDto documentDto, CancellationToken cancellationToken);
        Task DeleteDocumentAsync(int documentId, CancellationToken cancellationToken);
    }
}
