using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using System.Transactions;
using WarehouseManagementApi.Infrastructure.Services;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.DocumentCommon;
using WarehouseManagementApi.Models.SnipmentDocument;

namespace WarehouseManagementApi.Infrastructure.Abstraction
{
    public interface ISnipmentDocumentService
    {
        Task<SnipmentFilterData> GetFilterData(CancellationToken cancellationToken);
        Task<IEnumerable<SnipmentDocumentViewDto>> GetDocuments(SnipmentDocumentFilter filter, CancellationToken cancellationToken);
        Task<SnipmentResourceReferences> GetReferences(int? documentId = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<SnipmentResourceDto>> GetResourceBalance(CancellationToken cancellationToken);
        Task<SnipmentDocumentDto> GetDocument(int id, CancellationToken cancellationToken);
        Task CreateDocumentAsync(SnipmentDocumentDto documentDto, CancellationToken cancellationToken);
        Task UpdateDocumentAsync(SnipmentDocumentDto documentDto, CancellationToken cancellationToken);
        Task RevokeDocumentAsync(int documentId, CancellationToken cancellationToken);
        Task DeleteDocumentAsync(int documentId, CancellationToken cancellationToken);
    }
}
