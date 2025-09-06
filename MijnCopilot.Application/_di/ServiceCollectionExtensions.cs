using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddScoped<IGeneralAgentFactory, GeneralAgentFactory>();
        services.AddScoped<IMijnThuisAgentFactory, MijnThuisAgentFactory>();
        services.AddScoped<IMijnSaunaAgentFactory, MijnSaunaAgentFactory>();
        services.AddScoped<IPhotoCarouselAgentFactory, PhotoCarouselAgentFactory>();

        services.AddDataService();

        return services;
    }
}