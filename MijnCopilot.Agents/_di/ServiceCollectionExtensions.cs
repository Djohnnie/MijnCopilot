using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.Agents.Orchestration;

namespace MijnCopilot.Agents.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCopilotServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICopilotHelper, CopilotHelper>();
        services.AddScoped<IAgentFactory, AgentFactory>();
        services.AddScoped<KeywordAgentFactory>();
        services.AddScoped<SummaryAgentFactory>();
        services.AddScoped<OrchestratorAgentFactory>();
        services.AddScoped<QuestionAgentFactory>();
        services.AddScoped<ReplyAgentFactory>();
        services.AddScoped<GeneralAgentFactory>();
        services.AddScoped<MijnThuisPowerAgentFactory>();
        services.AddScoped<MijnThuisSolarAgentFactory>();
        services.AddScoped<MijnThuisCarAgentFactory>();
        services.AddScoped<MijnThuisHeatingAgentFactory>();
        services.AddScoped<MijnThuisSmartLockAgentFactory>();
        services.AddScoped<MijnSaunaAgentFactory>();
        services.AddScoped<PhotoCarouselAgentFactory>();
        services.AddScoped<IAgentOrchestrationManager, AgentOrchestrationManager>();

        return services;
    }
}