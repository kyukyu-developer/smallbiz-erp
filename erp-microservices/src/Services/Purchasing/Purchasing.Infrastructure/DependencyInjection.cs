using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchasing.Domain.Interfaces;
using Purchasing.Infrastructure.Data;
using Purchasing.Infrastructure.Repositories;
using Purchasing.Infrastructure.Services;

namespace Purchasing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PurchaseDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("PurchaseDb"),
                b => b.MigrationsAssembly(typeof(PurchaseDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHostedService<LowStockEventHandler>();

        return services;
    }
}
