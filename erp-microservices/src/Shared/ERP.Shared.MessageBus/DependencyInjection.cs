using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ERP.Shared.MessageBus;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IMessageBus>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<RabbitMqMessageBus>>();
            return new RabbitMqMessageBus(connectionString, logger);
        });

        return services;
    }
}
