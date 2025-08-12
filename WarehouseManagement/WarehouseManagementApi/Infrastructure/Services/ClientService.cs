using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Models;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Models;

namespace WarehouseManagementApi.Infrastructure.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;
        private readonly ISnipmentDocumentRepository snipmentDocumentRepository;
        public ClientService(IClientRepository clientRepository, ISnipmentDocumentRepository snipmentDocumentRepository)
        {
            this.clientRepository = clientRepository;
            this.snipmentDocumentRepository = snipmentDocumentRepository;
        }

        public async Task<ClientDto> GetAllClientAsync(bool isArchive, CancellationToken cancellationToken)
        {
            var clients = await clientRepository.GetAllAsync(isArchive, cancellationToken: cancellationToken);

            return new ClientDto
            {
                Items = clients.Select(x =>
                new ClientItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    IsArchive = x.Archive,
                }).ToList()
            };
        }

        public async Task<ClientItem> GetClientAsync(int id, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetByIdAsync(id, cancellationToken);

            return new ClientItem
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                IsArchive = client.Archive
            };
        }

        public async Task CreateClientAsync(ClientInsertRequestDto requestDto, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetByNameAsync(requestDto.Name, cancellationToken);

            if (client != null)
            {
                throw new InvalidOperationException($"Клиент с именем {requestDto.Name} уже существует!");
            }

            var newClient = new Client { Name = requestDto.Name, Address = requestDto.Address };

            await clientRepository.CreateAsync(newClient, cancellationToken);
        }

        public async Task EditStateAsync(ClientUpdateStateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var unit = await clientRepository.GetByIdAsync(requestDto.Id, cancellationToken);

            if (unit == null)
            {
                throw new KeyNotFoundException($"Клиент с ID {requestDto.Id} не найден!");
            }
            unit.Archive = requestDto.IsArchive;

            await clientRepository.UpdateAsync(unit, cancellationToken);
        }

        public async Task UpdateClientAsync(ClientUpdateRequestDto requestDto, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetByNameAsync(requestDto.Name, cancellationToken);

            if (client != null && client.Id != requestDto.Id)
            {
                throw new InvalidOperationException($"Клиент с названием {requestDto.Name} уже существует!");
            }

            client = await clientRepository.GetByIdAsync(requestDto.Id.Value, cancellationToken);

            if (client == null)
            {
                throw new KeyNotFoundException($"Клиент с ID {requestDto.Id} не найден!");
            }
            client.Name = requestDto.Name;
            client.Address = requestDto.Address;

            await clientRepository.UpdateAsync(client, cancellationToken);
        }

        public async Task DeleteClientAsync(int id, CancellationToken cancellationToken)
        {
            var snipmentDoc = await snipmentDocumentRepository.GetAllAsync(new DataAccessLayer.Infrastructure.FilterModel.SnipmentDocumentFilter { Clients = new List<int> { id } }, cancellationToken);

            if (snipmentDoc.Any())
            {
                throw new InvalidOperationException("Невозможно удалить клиента. У клиента есть отгрузки");
            }

            var deleteResource = await clientRepository.GetByIdAsync(id, cancellationToken);
            await clientRepository.DeleteAsync(deleteResource, cancellationToken);
        }
    }
}
