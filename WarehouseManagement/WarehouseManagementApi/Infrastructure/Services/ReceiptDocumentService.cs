using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Models;
using System.Transactions;
using WarehouseManagementApi.Exceptions;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.DocumentCommon;
using WarehouseManagementApi.Models.ReceiptDocument;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class ReceiptDocumentService : IReceiptDocumentService
    {
        private readonly IReceiptDocumentRepository receiptDocumentRepository;
        private readonly IReceiptResourceRepository receiptResourceRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IBalanceService balanceService;

        public ReceiptDocumentService(IReceiptDocumentRepository receiptDocumentRepository,
            IReceiptResourceRepository receiptResourceRepository,
            IResourceRepository resourceRepository,
            IUnitRepository unitRepository,
            IBalanceService balanceService)
        {
            this.receiptDocumentRepository = receiptDocumentRepository;
            this.receiptResourceRepository = receiptResourceRepository;
            this.resourceRepository = resourceRepository;
            this.unitRepository = unitRepository;
            this.balanceService = balanceService;
        }

        public async Task<ReceiptFilterData> GetFilterData(CancellationToken cancellationToken)
        {
            var units = await unitRepository.GetAllAsync(cancellationToken: cancellationToken);
            var resources = await resourceRepository.GetAllAsync(cancellationToken: cancellationToken);
            var documents = await receiptDocumentRepository.GetAllAsync(cancellationToken: cancellationToken);

            var result = new ReceiptFilterData
            {
                Units = units.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
                Resources = resources.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
                Numbers = documents.Select(x => new DropDownItem
                {
                    Code = x.Number,
                    Name = x.Number
                })
            };

            return result;
        }

        public async Task<IEnumerable<ReceiptDocumentViewDto>> GetDocuments(ReceiptDocumentFilter filter, CancellationToken cancellationToken)
        {
            var receiptDocuments = await receiptDocumentRepository.GetAllAsync(filter, cancellationToken);

            var result = receiptDocuments.Select(x => new ReceiptDocumentViewDto
            {
                Id = x.Id,
                Number = x.Number,
                Date = x.Date,
                DocumentResources = x.ReceiptResources.Select(y => new DocumentResource
                {
                    ResourceName = y.Resource.Name,
                    UnitName = y.Unit.Name,
                    Count = y.Count
                }).ToList()
            });

            return result;
        }

        public async Task<ReceiptResourceReferences> GetReferences(int? documentId = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<ReceiptResource> documentResources = Enumerable.Empty<ReceiptResource>();

            if (documentId.HasValue)
                documentResources = await receiptResourceRepository.GetResources(documentId.Value, cancellationToken);

            var units = await unitRepository.GetAllAsync(false, documentResources.Select(x => x.UnitId), cancellationToken);
            var resources = await resourceRepository.GetAllAsync(false, documentResources.Select(x => x.ResourceId), cancellationToken);

            return new ReceiptResourceReferences
            {
                Units = units.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
                Resources = resources.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                })
            };
        }

        public async Task<ReceiptDocumentDto> GetDocument(int id, CancellationToken cancellationToken)
        {
            var document = await receiptDocumentRepository.GetByIdAsync(id, cancellationToken);

            var result = new ReceiptDocumentDto
            {
                Id = document.Id,
                Number = document.Number,
                Date = document.Date,
                Resources = document.ReceiptResources.Select(y => new ReceiptResourceDto
                {
                    Id = y.Id,
                    ResourceId = y.ResourceId,
                    UnitId = y.UnitId,
                    Count = y.Count
                }).ToList()
            };

            return result;
        }

        public async Task CreateDocumentAsync(ReceiptDocumentDto documentDto, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await receiptDocumentRepository.GetByNumberAsync(documentDto.Number, cancellationToken);

                    if (document != null)
                    {
                        throw new InvalidOperationException($"Документ с номером {documentDto.Number} уже существует!");
                    }

                    var newDocument = new ReceiptDocument
                    {
                        Number = documentDto.Number,
                        Date = documentDto.Date
                    };

                    int documentId = await receiptDocumentRepository.CreateAsync(newDocument, cancellationToken);

                    if (documentDto.Resources != null && documentDto.Resources.Any())
                    {
                        CheckResources(documentDto.Resources);
                        await CreateResources(documentDto.Resources, documentId, cancellationToken);
                    }

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task CreateResources(IEnumerable<ReceiptResourceDto> resources, int documentId, CancellationToken cancellationToken)
        {
            var newResources = resources.Select(x => new ReceiptResource
            {
                DocumentId = documentId,
                ResourceId = x.ResourceId,
                UnitId = x.UnitId,
                Count = x.Count
            });

            await receiptResourceRepository.CreateRangeAsync(newResources, cancellationToken);

            foreach (var resource in resources)
            {
                await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, resource.Count * (-1), cancellationToken);
            }
        }

        private void CheckResources(IEnumerable<ReceiptResourceDto> resources)
        {
            var unCorrect = resources.Where(x => x.Count <= 0);
            if (unCorrect.Count() > 0) throw new InvalidOperationException("Неверно заполнены данные!");

            var duplicates = resources
                .GroupBy(x => new { x.ResourceId, x.UnitId })
                .Where(g => g.Count() > 1);
            if (duplicates.Any()) throw new InvalidOperationException("Нельзя установить один и тот же ресурс с одинаковыми единицами измерения!");
        }

        public async Task UpdateDocumentAsync(ReceiptDocumentDto documentDto, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await receiptDocumentRepository.GetByNumberAsync(documentDto.Number, cancellationToken);

                    if (document != null && document.Id != documentDto.Id)
                    {
                        throw new InvalidOperationException($"Документ с номером {documentDto.Number} уже существует!");
                    }

                    document = await receiptDocumentRepository.GetByIdAsync(documentDto.Id, cancellationToken);

                    if (document == null)
                    {
                        throw new KeyNotFoundException($"Документ с ID {documentDto.Id} не найден!");
                    }

                    document.Number = documentDto.Number;
                    document.Date = documentDto.Date;

                    await receiptDocumentRepository.UpdateAsync(document, cancellationToken);

                    if (documentDto.Resources != null && documentDto.Resources.Any())
                    {
                        CheckResources(documentDto.Resources.Where(x => !x.IsDelete));

                        await CreateResources(documentDto.Resources.Where(x => !x.Id.HasValue), documentDto.Id, cancellationToken);

                        await UpdateResources(documentDto.Resources.Where(x => x.Id.HasValue && x.IsChange), cancellationToken);

                        await DeleteResources(documentDto.Resources, documentDto.Resources.Where(x => x.IsDelete), cancellationToken);
                    }

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task UpdateResources(IEnumerable<ReceiptResourceDto> updateResources, CancellationToken cancellationToken)
        {
            foreach (var updateResource in updateResources)
            {
                try
                {
                    var resource = await receiptResourceRepository.GetByIdAsync(updateResource.Id.Value, cancellationToken);

                    await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, resource.Count - updateResource.Count, cancellationToken);

                    resource.ResourceId = updateResource.ResourceId;
                    resource.UnitId = updateResource.UnitId;
                    resource.Count = updateResource.Count;

                    await receiptResourceRepository.UpdateAsync(resource, cancellationToken);

                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidOperationException(ex.Message + "Редактирование невозможно");
                }
            }
        }
        private async Task DeleteResources(IEnumerable<ReceiptResourceDto> resources, IEnumerable<ReceiptResourceDto> deleteResources, CancellationToken cancellationToken)
        {
            foreach (var deleteResource in deleteResources)
            {
                var resource = await receiptResourceRepository.GetByIdAsync(deleteResource.Id.Value, cancellationToken);
                try
                {
                    await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, deleteResource.Count, cancellationToken);

                    await receiptResourceRepository.DeleteAsync(resource, cancellationToken);
                }
                catch (InvalidOperationException ex)
                {
                    throw new InvalidUpdateOperationException(ex.Message + "Удаление невозможно", resource.Id);
                }
            }
        }

        public async Task DeleteDocumentAsync(int documentId, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await receiptDocumentRepository.GetByIdAsync(documentId, cancellationToken);

                    if (document == null)
                    {
                        throw new KeyNotFoundException($"Документ с ID {documentId} не найден!");
                    }

                    var resources = document.ReceiptResources.ToList();

                    for (int i = 0; i < resources.Count; i++)
                    {
                        try
                        {
                            await receiptResourceRepository.DeleteAsync(resources[i], cancellationToken);
                            await balanceService.UpdateBalanceAsync(resources[i].ResourceId, resources[i].UnitId, resources[i].Count, cancellationToken);
                        }
                        catch (InvalidOperationException ex)
                        {
                            throw new InvalidOperationException(ex.Message + "Удаление невозможно");

                        }
                    }

                    await receiptDocumentRepository.DeleteAsync(document, cancellationToken);

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
