using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.Application.Helpers.Agents;
using MijnCopilot.Application.Helpers;
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

        services.AddScoped<ICopilotHelper, CopilotHelper>();
        services.AddScoped<AgentFactory>();
        services.AddScoped<IKeywordAgentFactory, KeywordAgentFactory>();
        services.AddScoped<ISummaryAgentFactory, SummaryAgentFactory>();
        services.AddScoped<IOrchestratorAgentFactory, OrchestratorAgentFactory>();
        services.AddScoped<IGeneralAgentFactory, GeneralAgentFactory>();
        services.AddScoped<IMijnThuisPowerAgentFactory, MijnThuisPowerAgentFactory>();
        services.AddScoped<IMijnThuisSolarAgentFactory, MijnThuisSolarAgentFactory>();
        services.AddScoped<IMijnThuisCarAgentFactory, MijnThuisCarAgentFactory>();
        services.AddScoped<IMijnThuisHeatingAgentFactory, MijnThuisHeatingAgentFactory>();
        services.AddScoped<IMijnThuisSmartLockAgentFactory, MijnThuisSmartLockAgentFactory>();
        services.AddScoped<IMijnSaunaAgentFactory, MijnSaunaAgentFactory>();
        services.AddScoped<IPhotoCarouselAgentFactory, PhotoCarouselAgentFactory>();

        services.AddDataService();

        return services;
    }
}