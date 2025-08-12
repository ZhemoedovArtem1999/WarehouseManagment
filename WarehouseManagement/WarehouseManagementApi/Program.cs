
using DataAccessLayer.Data;
using DataAccessLayer.Dependency;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using WarehouseManagementApi.Infrastructure.Abstraction;
using WarehouseManagementApi.Infrastructure.Services;

namespace WarehouseManagement
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(serverOptions => {
                serverOptions.ListenAnyIP(5098); 
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost3001", builder =>
                {
                    builder.WithOrigins("http://localhost:3001")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WarehouseManagmentApi", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.CustomOperationIds(apiDesc =>
                {
                    return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)
                        ? methodInfo.Name
                        : null;
                });



            });

            builder.Services.ConfigureServices(builder.Configuration);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseCors("AllowLocalhost3001");

            app.MapControllers();

            await app.RunAsync();
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.DependencyInjectionDataAccessLayer(configuration);

            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IReceiptDocumentService, ReceiptDocumentService>();
            services.AddScoped<ISnipmentDocumentService, SnipmentDocumentService>();
            services.AddScoped<IBalanceService, BalanceService>();

            return services;
        }
    }
}
