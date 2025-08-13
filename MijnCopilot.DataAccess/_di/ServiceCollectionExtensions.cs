using Microsoft.Extensions.DependencyInjection;

namespace MijnCopilot.DataAccess.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        services.AddDbContext<MijnCopilotDbContext>();

        return services;
    }
}