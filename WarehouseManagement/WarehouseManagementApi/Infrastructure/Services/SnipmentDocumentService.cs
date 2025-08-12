using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.FilterModel;
using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using System.Linq.Expressions;
using System.Threading;
using System.Transactions;
using WarehouseManagementApi.Exceptions;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;
using WarehouseManagementApi.Models.DocumentCommon;
using WarehouseManagementApi.Models.SnipmentDocument;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class SnipmentDocumentService : ISnipmentDocumentService
    {
        private readonly ISnipmentDocumentRepository snipmentDocumentRepository;
        private readonly ISnipmentResourceRepository snipmentResourceRepository;
        private readonly IResourceRepository resourceRepository;
        private readonly IUnitRepository unitRepository;
        private readonly IClientRepository clientRepository;
        private readonly IBalanceService balanceService;

        public SnipmentDocumentService(ISnipmentDocumentRepository snipmentDocumentRepository,
            ISnipmentResourceRepository snipmentResourceRepository,
            IResourceRepository resourceRepository,
            IUnitRepository unitRepository,
            IClientRepository clientRepository,
            IBalanceService balanceService)
        {
            this.snipmentDocumentRepository = snipmentDocumentRepository;
            this.snipmentResourceRepository = snipmentResourceRepository;
            this.resourceRepository = resourceRepository;
            this.unitRepository = unitRepository;
            this.clientRepository = clientRepository;
            this.balanceService = balanceService;
        }

        public async Task<SnipmentFilterData> GetFilterData(CancellationToken cancellationToken)
        {
            var units = await unitRepository.GetAllAsync(cancellationToken: cancellationToken);
            var resources = await resourceRepository.GetAllAsync(cancellationToken: cancellationToken);
            var clients = await clientRepository.GetAllAsync(cancellationToken: cancellationToken);
            var documents = await snipmentDocumentRepository.GetAllAsync(cancellationToken: cancellationToken);

            var result = new SnipmentFilterData
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
                Clients = clients.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name
                }),
                Numbers = documents.Select(x => new DropDownItem
                {
                    Code = x.Number,
                    Name = x.Number
                })
            };

            return result;
        }

        public async Task<IEnumerable<SnipmentDocumentViewDto>> GetDocuments(SnipmentDocumentFilter filter, CancellationToken cancellationToken)
        {
            var snipmentDocuments = await snipmentDocumentRepository.GetAllAsync(filter, cancellationToken);

            var result = snipmentDocuments.Select(x => new SnipmentDocumentViewDto
            {
                Id = x.Id,
                Number = x.Number,
                Date = x.Date,
                Client = x.Client.Name,
                Sign = x.Sign,
                DocumentResources = x.SnipmentResources.Select(y => new DocumentResource
                {
                    ResourceName = y.Resource.Name,
                    UnitName = y.Unit.Name,
                    Count = y.Count
                }).ToList()
            });

            return result;
        }

        public async Task<SnipmentResourceReferences> GetReferences(int? documentId = null, CancellationToken cancellationToken = default)
        {
            IEnumerable<SnipmentResource> documentResources = Enumerable.Empty<SnipmentResource>();

            if (documentId.HasValue)
                documentResources = await snipmentResourceRepository.GetResources(documentId.Value, cancellationToken);

            var clients = await clientRepository.GetAllAsync(false, documentResources.Select(x => x.ResourceId), cancellationToken);

            return new SnipmentResourceReferences
            {
                Clients = clients.Select(x => new DropDownItem
                {
                    Code = x.Id.ToString(),
                    Name = x.Name,
                }),
            };
        }

        public async Task<SnipmentDocumentDto> GetDocument(int id, CancellationToken cancellationToken)
        {
            var document = await snipmentDocumentRepository.GetByIdAsync(id, cancellationToken);
            var balance = await balanceService.GetBalance(null, cancellationToken);

            var result = new SnipmentDocumentDto
            {
                Id = document.Id,
                Number = document.Number,
                Date = document.Date,
                IsSign = document.Sign,
                ClientId = document.ClientId,
                Resources = balance.Select(x => new SnipmentResourceDto
                {
                    Id = document.SnipmentResources.FirstOrDefault(y => y.ResourceId == x.ResourceId && y.UnitId == x.UnitId)?.Id,
                    ResourceId = x.ResourceId,
                    ResourceName = x.ResourceName,
                    UnitId = x.UnitId,
                    UnitName = x.UnitName,
                    Count = document.SnipmentResources.FirstOrDefault(y => y.ResourceId == x.ResourceId && y.UnitId == x.UnitId)?.Count ?? decimal.Zero,
                    CountBalance = x.Count
                })
                .Concat(document.SnipmentResources
                .Where(x => !balance.Any(b => b.ResourceId == x.ResourceId && b.UnitId == x.UnitId))
                .Select(x => new SnipmentResourceDto
                {
                    Id = x.Id,
                    ResourceId = x.ResourceId,
                    ResourceName = x.Resource.Name,
                    UnitId = x.UnitId,
                    UnitName = x.Unit.Name,
                    Count = x.Count,
                    CountBalance = 0
                })).OrderBy(x => x.ResourceName),
            };

            return result;
        }

        public async Task<IEnumerable<SnipmentResourceDto>> GetResourceBalance(CancellationToken cancellationToken)
        {
            var balance = await balanceService.GetBalance(null, cancellationToken);

            if (balance == null || !balance.Any())
                throw new InvalidOperationException("На балансе нет ресурсов для отгрузки");

            var result = balance.Select(x => new SnipmentResourceDto
            {
                ResourceId = x.ResourceId,
                ResourceName = x.ResourceName,
                UnitId = x.UnitId,
                UnitName = x.UnitName,
                CountBalance = x.Count
            }).OrderBy(x => x.ResourceName);

            return result;
        }

        public async Task CreateDocumentAsync(SnipmentDocumentDto documentDto, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await snipmentDocumentRepository.GetByNumberAsync(documentDto.Number, cancellationToken);

                    if (document != null)
                    {
                        throw new InvalidOperationException($"Документ с номером {documentDto.Number} уже существует!");
                    }

                    if (documentDto.Resources == null || documentDto.Resources.Where(x => x.Count != 0).Count() == 0)
                    {
                        throw new InvalidOperationException($"Заполните ресурсы. Отгрузка не должна быть пустой.");
                    }

                    var newDocument = new SnipmentDocument
                    {
                        Number = documentDto.Number,
                        Date = documentDto.Date,
                        Sign = documentDto.IsSign,
                        ClientId = documentDto.ClientId
                    };

                    if (documentDto.IsSign)
                    {
                        await Sign(documentDto.Resources, cancellationToken);
                    }

                    int documentId = await snipmentDocumentRepository.CreateAsync(newDocument, cancellationToken);

                    if (documentDto.Resources != null && documentDto.Resources.Any())
                    {
                        await CreateResources(documentDto.Resources.Where(x => x.Count > 0), documentId, cancellationToken);
                    }

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task CreateResources(IEnumerable<SnipmentResourceDto> resources, int documentId, CancellationToken cancellationToken)
        {
            var newResources = resources.Select(x => new SnipmentResource
            {
                DocumentId = documentId,
                ResourceId = x.ResourceId,
                UnitId = x.UnitId,
                Count = x.Count
            });

            await snipmentResourceRepository.CreateRangeAsync(newResources, cancellationToken);
        }

        private async Task Sign(IEnumerable<SnipmentResourceDto> resources, CancellationToken cancellationToken)
        {
            foreach (var resource in resources)
            {
                SnipmentResource currentResource = null;
                if (resource.Id.HasValue)
                {
                    currentResource = await snipmentResourceRepository.GetByIdAsync(resource.Id.Value, cancellationToken);
                }

                if (currentResource == null)
                {
                    await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, resource.Count, cancellationToken);
                }
                else
                {
                    if (resource.IsChange)
                    {
                        await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, resource.Count, cancellationToken);
                    }
                    else
                    {
                        await balanceService.UpdateBalanceAsync(resource.ResourceId, resource.UnitId, currentResource.Count, cancellationToken);
                    }
                }
            }
        }

        public async Task UpdateDocumentAsync(SnipmentDocumentDto documentDto, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await snipmentDocumentRepository.GetByNumberAsync(documentDto.Number, cancellationToken);

                    if (document != null && document.Id != documentDto.Id)
                    {
                        throw new InvalidOperationException($"Документ с номером {documentDto.Number} уже существует!");
                    }

                    document = await snipmentDocumentRepository.GetByIdAsync(documentDto.Id, cancellationToken);

                    if (document == null)
                    {
                        throw new KeyNotFoundException($"Документ с ID {documentDto.Id} не найден!");
                    }

                    if (documentDto.Resources.All(x => x.Count == 0))
                    {
                        throw new InvalidOperationException($"Заполните ресурсы. Отгрузка не должна быть пустой.");
                    }

                    document.Number = documentDto.Number;
                    document.Date = documentDto.Date;
                    document.Sign = documentDto.IsSign;
                    document.ClientId = documentDto.ClientId;

                    await snipmentDocumentRepository.UpdateAsync(document, cancellationToken);

                    if (documentDto.IsSign)
                    {
                        await Sign(documentDto.Resources, cancellationToken);
                    }

                    if (documentDto.Resources != null && documentDto.Resources.Any())
                    {
                        await UpdateResources(documentDto.Resources.Where(x => x.IsChange), documentDto.Id, cancellationToken);
                    }

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        private async Task UpdateResources(IEnumerable<SnipmentResourceDto> updateResources, int documentId, CancellationToken cancellationToken)
        {
            foreach (var updateResource in updateResources)
            {
                if (!updateResource.Id.HasValue)
                {
                    await this.CreateResources(new List<SnipmentResourceDto>() { updateResource }, documentId, cancellationToken);
                    continue;
                }
                var resource = await snipmentResourceRepository.GetByIdAsync(updateResource.Id.Value, cancellationToken);

                if (updateResource.Count == 0)
                {
                    await snipmentResourceRepository.DeleteAsync(resource, cancellationToken);
                }
                else
                {
                    resource.ResourceId = updateResource.ResourceId;
                    resource.UnitId = updateResource.UnitId;
                    resource.Count = updateResource.Count;

                    await snipmentResourceRepository.UpdateAsync(resource, cancellationToken);
                }
            }
        }

        public async Task RevokeDocumentAsync(int documentId, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await snipmentDocumentRepository.GetByIdAsync(documentId, cancellationToken);

                    if (document == null)
                    {
                        throw new KeyNotFoundException($"Документ с ID {documentId} не найден!");
                    }

                    var resources = document.SnipmentResources.ToList();

                    for (int i = 0; i < resources.Count; i++)
                    {
                        await balanceService.UpdateBalanceAsync(resources[i].ResourceId, resources[i].UnitId, resources[i].Count * (-1), cancellationToken);
                    }

                    document.Sign = false;

                    await snipmentDocumentRepository.UpdateAsync(document, cancellationToken);

                    scope.Complete();
                }
                catch
                {
                    throw;
                }
            }
        }

        public async Task DeleteDocumentAsync(int documentId, CancellationToken cancellationToken)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var document = await snipmentDocumentRepository.GetByIdAsync(documentId, cancellationToken);

                    if (document == null)
                    {
                        throw new KeyNotFoundException($"Документ с ID {documentId} не найден!");
                    }

                    var resources = document.SnipmentResources.ToList();

                    for (int i = 0; i < resources.Count; i++)
                    {
                        await snipmentResourceRepository.DeleteAsync(resources[i], cancellationToken);
                    }

                    await snipmentDocumentRepository.DeleteAsync(document, cancellationToken);

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
