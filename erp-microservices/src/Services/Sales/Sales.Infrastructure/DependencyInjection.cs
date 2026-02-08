using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Interfaces;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Data;
using Sales.Infrastructure.Repositories;
using Sales.Infrastructure.Services;

namespace Sales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<SalesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SalesDb"),
                b => b.MigrationsAssembly(typeof(SalesDbContext).Assembly.FullName)));

        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register HttpClient for InventoryService
        var inventoryBaseUrl = configuration["ServiceUrls:InventoryServiceUrl"]
            ?? "http://localhost:5002";

        services.AddHttpClient<IInventoryService, InventoryHttpService>(client =>
        {
            client.BaseAddress = new Uri(inventoryBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}
