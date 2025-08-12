using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Repository;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
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
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_connectionString)
                .Options;



            _dbContext = new AppDbContext(options);
            _dbContext.Database.EnsureCreated();
            SeedInitialData();

            CleanDatabase();

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
        public void CleanDatabase()
        {
            _dbContext.ReceiptResources.RemoveRange(_dbContext.ReceiptResources);
            _dbContext.SnipmentResources.RemoveRange(_dbContext.SnipmentResources);
            _dbContext.SnipmentDocuments.RemoveRange(_dbContext.SnipmentDocuments);
            _dbContext.ReceiptDocuments.RemoveRange(_dbContext.ReceiptDocuments);
            _dbContext.Balances.RemoveRange(_dbContext.Balances);
            _dbContext.SaveChanges();
        }

        private void SeedInitialData()
        {
            if (!_dbContext.Clients.Any())
            {
                _dbContext.Clients.Add(new Client { Id = 1, Name = "Test Client", Address = "Address Test" });
            }

            if (!_dbContext.Resources.Any())
            {
                _dbContext.Resources.Add(new Resource { Id = 1, Name = "Test Resource" });
            }

            if (!_dbContext.UnitMeasurements.Any())
            {
                _dbContext.UnitMeasurements.Add(new UnitMeasurement { Id = 1, Name = "Test Unit" });
            }

            _dbContext.SaveChanges();
        }

        [Test]
        public async Task ConcurrentOperations_ShouldMaintainDataConsistency()
        {
            // Arrange
            await _receiptDocumentService.CreateDocumentAsync(
             new ReceiptDocumentDto
             {
                 Number = "REC-002",
                 Date = DateOnly.FromDateTime(DateTime.Today),
                 Resources = new List<ReceiptResourceDto>
                 {
                        new() { ResourceId = 1, UnitId = 1, Count = 50 }
                 }
             },
             CancellationToken.None);

            var receiptTasks = Enumerable.Range(0, 10).SelectMany(i => new[]{
                Task.Run(async () =>
            {
                await _receiptDocumentService.CreateDocumentAsync(
                    new ReceiptDocumentDto
                    {
                        Number = "REC-002",
                        Date = DateOnly.FromDateTime(DateTime.Today),
                        Resources = new List<ReceiptResourceDto>
                        {
                        new() { ResourceId = 1, UnitId = 1, Count = 50 }
                        }
                    },
                    CancellationToken.None);
            }) });

            var snipmentTasks = Enumerable.Range(0, 10).SelectMany(i => new[]{
                Task.Run(async () =>
            {
           await _snipmentDocumentService.CreateDocumentAsync(
                    new SnipmentDocumentDto
                    {
                        Number = "SHIP-002",
                        Date = DateOnly.FromDateTime(DateTime.Today),
                        ClientId = 1,
                        IsSign = true,
                        Resources = new List<SnipmentResourceDto>
                        {
                        new() { ResourceId = 1, UnitId = 1, Count = 20 }
                        }
                    },
                    CancellationToken.None);
            }) });

            var operations = snipmentTasks.Concat(receiptTasks);


            // Act
            await Task.WhenAll(operations);

            // Assert
            var balance = await _dbContext.Balances
                .FirstOrDefaultAsync(b => b.ResourceId == 1 && b.UnitId == 1);

            Assert.That(balance, Is.Not.Null);
            Assert.That(balance.Count, Is.EqualTo(30)); // 50 - 20 = 30
        }

        public void Dispose()
        {
            CleanDatabase();
            _dbContext.Clients.Remove(new Client { Id = 1, Name = "Test Client", Address = "Address Test" });
            _dbContext.Resources.Remove(new Resource { Id = 1, Name = "Test Resource" });
            _dbContext.UnitMeasurements.Remove(new UnitMeasurement { Id = 1, Name = "Test Unit" });
            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }

}

