using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models.Unit;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class UnitService:IUnitService
    {
        private readonly IUnitRepository unitRepository;
        private readonly IReceiptResourceRepository receiptResourceRepository;
        private readonly ISnipmentResourceRepository snipmentResourceRepository;
        public UnitService(IUnitRepository unitRepository, IReceiptResourceRepository receiptResourceRepository, ISnipmentResourceRepository snipmentResourceRepository)
        {
            this.unitRepository = unitRepository;
            this.receiptResourceRepository = receiptResourceRepository;
            this.snipmentResourceRepository = snipmentResourceRepository;
        }

        public async Task<UnitDto> GetAllUnitAsync(bool isArchive, CancellationToken cancellationToken)
        {
            var units = await unitRepository.GetAllAsync(isArchive, cancellationToken: cancellationToken);

            return new UnitDto
            {
                Items = units.Select(x =>
                new UnitItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsArchive = x.Archive,
                }).ToList()
            };
        }

        public async Task<UnitItem> GetUnitAsync(int id, CancellationToken cancellationToken)
        {
            var unit = await unitRepository.GetByIdAsync(id, cancellationToken);

            return new UnitItem
            {
                Id = unit.Id,
                Name = unit.Name,
                IsArchive = unit.Archive
            };
        }

        public async Task CreateUnitAsync(string name, CancellationToken cancellationToken)
        {
            var unit = await unitRepository.GetByNameAsync(name, cancellationToken);

            if (unit != null)
            {
                throw new InvalidOperationException($"Единица измерения с названием {name} уже существует!");
            }

            var newUnit = new UnitMeasurement { Name = name };

            await unitRepository.CreateAsync(newUnit, cancellationToken);
        }

        public async Task EditStateAsync(UnitUpdateStateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var unit = await unitRepository.GetByIdAsync(requestDto.Id, cancellationToken);

            if (unit == null)
            {
                throw new KeyNotFoundException($"Единица измерения с ID {requestDto.Id} не найден!");
            }
            unit.Archive = requestDto.IsArchive;

            await unitRepository.UpdateAsync(unit, cancellationToken);
        }

        public async Task UpdateUnitAsync(UnitUpdateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var resource = await unitRepository.GetByNameAsync(requestDto.Name, cancellationToken);

            if (resource != null && resource.Id != requestDto.Id)
            {
                throw new InvalidOperationException($"Единица измерения с названием {requestDto.Name} уже существует!");
            }

            resource = await unitRepository.GetByIdAsync(requestDto.Id, cancellationToken);

            if (resource == null)
            {
                throw new KeyNotFoundException($"Единица измерения с ID {requestDto.Id} не найден!");
            }
            resource.Name = requestDto.Name;

            await unitRepository.UpdateAsync(resource, cancellationToken);
        }

        public async Task DeleteUnitAsync(int id, CancellationToken cancellationToken)
        {
            var receiptRes = await receiptResourceRepository.GetResourceByUnitId(id, cancellationToken);
            var snipmentRes = await snipmentResourceRepository.GetResourceByUnitId(id, cancellationToken);

            if ( receiptRes.Any() || snipmentRes.Any())
            {
                throw new InvalidOperationException("Невозможно удалить единицу измерения. Единица измерения используется");
            }

            var deleteResource = await unitRepository.GetByIdAsync(id, cancellationToken);
            await unitRepository.DeleteAsync(deleteResource, cancellationToken);
        }
    }
}
