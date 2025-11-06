using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.Agents.DependencyInjection;
using MijnCopilot.DataAccess.DependencyInjection;
using System.Reflection;

namespace MijnCopilot.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var licenseKey = configuration.GetValue<string>("MEDIATR_LICENSEKEY");

        services.AddMediatR(c =>
        {
            c.LicenseKey = licenseKey;
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddCopilotServices(configuration);
        services.AddDataService();

        return services;
    }
}