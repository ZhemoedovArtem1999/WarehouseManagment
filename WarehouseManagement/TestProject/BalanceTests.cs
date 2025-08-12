using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementApi.Infrastructure.Services;
using WarehouseManagementApi.Models.ReceiptDocument;
using WarehouseManagementApi.Models.SnipmentDocument;

namespace TestsService
{
    [TestFixture]
    public class BalanceTests : IDisposable
    {
        private AppDbContext _dbContext;
        private ReceiptDocumentService _receiptDocumentService;
        private SnipmentDocumentService _snipmentDocumentService;
        private readonly string _connectionString = "Host=localhost;Port=12578;Username=postgres;Password=postgres;Database=warehouse;";

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;



            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureCreated();

            await CleanDatabase();

            // Инициализация сервисов
            var receiptDocRepo = new ReceiptDocumentRepository(_dbContext);
            var receiptResourceRepo = new ReceiptResourceRepository(_dbContext);
            var snipmentDocRepo = new SnipmentDocumentRepository(_dbContext);
            var snipmentResourceRepo = new SnipmentResourceRepository(_dbContext);
            var unitRepo = new UnitRepository(_dbContext);
            var resRepo = new ResourceRepository(_dbContext);
            var clientRepo = new ClientRepository(_dbContext);
            var balanceRepo = new BalanceRepository(_dbContext);
            var balanceService = new BalanceService(balanceRepo, resRepo, unitRepo);

            _receiptDocumentService = new ReceiptDocumentService(
                receiptDocRepo,
                receiptResourceRepo,
                resRepo,
                unitRepo,
                balanceService
                );

            _snipmentDocumentService = new SnipmentDocumentService(
                snipmentDocRepo,
                snipmentResourceRepo,
                resRepo,
                unitRepo,
                clientRepo,
                balanceService);
        }

        [TearDown]
        public async Task CleanDatabase()
        {
            _dbContext.ReceiptResources.RemoveRange(_dbContext.ReceiptResources);
            _dbContext.SnipmentResources.RemoveRange(_dbContext.SnipmentResources);
            _dbContext.SnipmentDocuments.RemoveRange(_dbContext.SnipmentDocuments);
            _dbContext.ReceiptDocuments.RemoveRange(_dbContext.ReceiptDocuments);
            _dbContext.Balances.RemoveRange(_dbContext.Balances);
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedInitialData()
        {
            await _dbContext.Clients.AddAsync(new Client { Id = 1, Name = "Test Client", Address = "Address Test" });
            await _dbContext.Resources.AddAsync(new Resource { Id = 1, Name = "Test Resource" });
            await _dbContext.UnitMeasurements.AddAsync(new UnitMeasurement { Id = 1, Name = "Test Unit" });
            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task ConcurrentOperations_ShouldMaintainDataConsistency()
        {
            // Arrange
            await SeedInitialData();

            var receiptTasks = new List<Task>();
            var shipmentTasks = new List<Task>();
            try
            {

            await _receiptDocumentService.CreateDocumentAsync(
             new ReceiptDocumentDto
             {
                 Number = "REC-000",
                 Date = DateOnly.FromDateTime(DateTime.Today),
                 Resources = new List<ReceiptResourceDto>
                 {
                        new() { ResourceId = 1, UnitId = 1, Count = 300 }
                 }
             },
             CancellationToken.None);

            for (int i = 0; i < 10; i++)
            {
                receiptTasks.Add(ExecuteReceiptOperationAsync());
                shipmentTasks.Add(ExecuteShipmentOperationAsync());
            }

                // Act - ждем ВСЕ задачи
                Console.WriteLine($"Запущено задач: receipt={receiptTasks.Count}, shipment={shipmentTasks.Count}");
                var allTasks = receiptTasks.Concat(shipmentTasks).ToList();
                await Task.WhenAll(allTasks);
                var success = allTasks.Count(t => t.IsCompletedSuccessfully);
                var a = 5;
            }
            catch
            {
                Console.WriteLine();
            }


            // Assert
            var balance = await _dbContext.Balances
                .FirstOrDefaultAsync(b => b.ResourceId == 1 && b.UnitId == 1);

            Assert.That(balance, Is.Not.Null);
            Assert.That(balance.Count, Is.EqualTo(800 - 200));
        }

        private async Task ExecuteReceiptOperationAsync()
        {
            await using var context = CreateNewDbContext();
            var service = CreateReceiptService(context);

            await service.CreateDocumentAsync(
                new ReceiptDocumentDto
                {
                    Number = $"REC-{Guid.NewGuid()}",
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    Resources = new List<ReceiptResourceDto>
                    {
                new() { ResourceId = 1, UnitId = 1, Count = 50 }
                    }
                },
                CancellationToken.None);
        }

        private async Task ExecuteShipmentOperationAsync()
        {
            await using var context = CreateNewDbContext();
            var service = CreateShipmentService(context);

            await service.CreateDocumentAsync(
                new SnipmentDocumentDto
                {
                    Number = $"SHIP-{Guid.NewGuid()}",
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    ClientId = 1,
                    IsSign = true,
                    Resources = new List<SnipmentResourceDto>
                    {
                new() { ResourceId = 1, UnitId = 1, Count = 20 }
                    }
                },
                CancellationToken.None);
        }

        private AppDbContext CreateNewDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString) 
                .Options;


            return new AppDbContext(options); 
        }

        private ReceiptDocumentService CreateReceiptService(AppDbContext context)
        {
            return new ReceiptDocumentService(
                new ReceiptDocumentRepository(context),
                new ReceiptResourceRepository(context),
                new ResourceRepository(context),
                new UnitRepository(context),
                new BalanceService(
                    new BalanceRepository(context),
                    new ResourceRepository(context),
                    new UnitRepository(context))
            );
        }

        private SnipmentDocumentService CreateShipmentService(AppDbContext context)
        {
            return new SnipmentDocumentService(
                new SnipmentDocumentRepository(context),
                new SnipmentResourceRepository(context),
                new ResourceRepository(context),
                new UnitRepository(context),
                new ClientRepository(context),
                new BalanceService(
                    new BalanceRepository(context),
                    new ResourceRepository(context),
                    new UnitRepository(context))
            );
        }

        public void Dispose()
        {
            CleanDatabase();

            var client = _dbContext.Clients.FirstOrDefault(x => x.Id == 1);
            if (client != null)
                _dbContext.Clients.Remove(client);
            var res = _dbContext.Resources.FirstOrDefault(x => x.Id == 1);
            if (res != null)
                _dbContext.Resources.Remove(res);
            var unit = _dbContext.UnitMeasurements.FirstOrDefault(x => x.Id == 1);
            if (unit != null)
                _dbContext.UnitMeasurements.Remove(unit);
            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }

}

