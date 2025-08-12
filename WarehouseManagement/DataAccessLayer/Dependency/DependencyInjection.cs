using DataAccessLayer.Data;
using DataAccessLayer.Infrastructure.Abstraction;
using DataAccessLayer.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection DependencyInjectionDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MainDb");

            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IReceiptDocumentRepository, ReceiptDocumentRepository>();
            services.AddScoped<IReceiptResourceRepository, ReceiptResourceRepository>();
            services.AddScoped<ISnipmentDocumentRepository, SnipmentDocumentRepository>();
            services.AddScoped<ISnipmentResourceRepository, SnipmentResourceRepository>();
            services.AddScoped<IBalanceRepository, BalanceRepository>();

            return services;
        }
    }
}
