using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Repositories;
using Inventory.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("InventoryDb"),
                b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<StockEventHandler>();

        return services;
    }
}
