using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using System.IO.Compression;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository resourceRepository;
        private readonly IReceiptResourceRepository receiptResourceRepository;
        private readonly ISnipmentResourceRepository snipmentResourceRepository;
        public ResourceService(IResourceRepository resourceRepository, IReceiptResourceRepository receiptResourceRepository, ISnipmentResourceRepository snipmentResourceRepository)
        {
            this.resourceRepository = resourceRepository;
            this.receiptResourceRepository = receiptResourceRepository;
            this.snipmentResourceRepository = snipmentResourceRepository;
        }

        public async Task<ResourceDto> GetAllResourceAsync(bool isArchive, CancellationToken cancellationToken)
        {
            var resources = await resourceRepository.GetAllAsync(isArchive, cancellationToken: cancellationToken);

            return new ResourceDto
            {
                Items = resources.Select(x =>
                new ResourceItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsArchive = x.Archive,
                }).ToList()
            };
        }

        public async Task<ResourceItem> GetResourceAsync(int id, CancellationToken cancellationToken)
        {
            var resource = await resourceRepository.GetByIdAsync(id, cancellationToken);

            return new ResourceItem
            {
               Id = resource.Id,
               Name = resource.Name,
               IsArchive = resource.Archive
            };
        }

        public async Task CreateResourceAsync(string name, CancellationToken cancellationToken)
        {
            var resource = await resourceRepository.GetByNameAsync(name, cancellationToken);

            if (resource != null)
            {
                throw new InvalidOperationException($"Ресурс с названием {name} уже существует!");
            }

            var newResource = new Resource { Name = name };

            await resourceRepository.CreateAsync(newResource, cancellationToken);
        }

        public async Task EditStateAsync(ResourceUpdateStateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var resource = await resourceRepository.GetByIdAsync(requestDto.Id, cancellationToken);

            if (resource == null)
            {
                throw new KeyNotFoundException($"Ресурс с ID {requestDto.Id} не найден!");
            }
            resource.Archive = requestDto.IsArchive;

            await resourceRepository.UpdateAsync(resource, cancellationToken);
        }

        public async Task UpdateResourceAsync(ResourceUpdateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var resource = await resourceRepository.GetByNameAsync(requestDto.Name, cancellationToken);

            if (resource != null && resource.Id != requestDto.Id)
            {
                throw new InvalidOperationException($"Ресурс с названием {requestDto.Name} уже существует!");
            }

            resource = await resourceRepository.GetByIdAsync(requestDto.Id, cancellationToken);

            if (resource == null)
            {
                throw new KeyNotFoundException($"Ресурс с ID {requestDto.Id} не найден!");
            }
            resource.Name = requestDto.Name;

            await resourceRepository.UpdateAsync(resource, cancellationToken);
        }

        public async Task DeleteResource(int id, CancellationToken cancellationToken)
        {
            var receiptRes = await receiptResourceRepository.GetResourceByResourceId(id, cancellationToken);
            var snipmentRes = await snipmentResourceRepository.GetResourceByResourceId(id, cancellationToken);

            if (receiptRes.Any() || snipmentRes.Any())
            {
                throw new InvalidOperationException("Невозможно удалить ресурс. Ресурс используется");
            }

            var deleteResource = await resourceRepository.GetByIdAsync(id, cancellationToken);
            await resourceRepository.DeleteAsync(deleteResource, cancellationToken);
        }
    }
}
