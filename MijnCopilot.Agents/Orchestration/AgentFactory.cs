using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents.Orchestration;

internal enum AgentType
{
    Keyword,
    Summary,
    Orchestrator,
    Question,
    Reply,
    General,
    MijnThuisPower,
    MijnThuisSolar,
    MijnThuisCar,
    MijnThuisHeating,
    MijnThuisSmartLock,
    MijnSauna,
    PhotoCarousel,
}

internal interface IAgentFactory
{
    Task<CopilotAgent> Create(AgentType type);
}

internal class AgentFactory : IAgentFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AgentFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<CopilotAgent> Create(AgentType type)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        AgentFactoryBase agent = type switch
        {
            AgentType.Keyword => scope.ServiceProvider.GetRequiredService<KeywordAgentFactory>(),
            AgentType.Summary => scope.ServiceProvider.GetRequiredService<SummaryAgentFactory>(),
            AgentType.Orchestrator => scope.ServiceProvider.GetRequiredService<OrchestratorAgentFactory>(),
            AgentType.Question => scope.ServiceProvider.GetRequiredService<QuestionAgentFactory>(),
            AgentType.Reply => scope.ServiceProvider.GetRequiredService<ReplyAgentFactory>(),
            AgentType.General => scope.ServiceProvider.GetRequiredService<GeneralAgentFactory>(),
            //AgentType.MijnThuisPower => scope.ServiceProvider.GetRequiredService<MijnThuisPowerAgentFactory>(),
            //AgentType.MijnThuisSolar => scope.ServiceProvider.GetRequiredService<MijnThuisSolarAgentFactory>(),
            //AgentType.MijnThuisCar => scope.ServiceProvider.GetRequiredService<MijnThuisCarAgentFactory>(),
            //AgentType.MijnThuisHeating => scope.ServiceProvider.GetRequiredService<MijnThuisHeatingAgentFactory>(),
            //AgentType.MijnThuisSmartLock => scope.ServiceProvider.GetRequiredService<MijnThuisSmartLockAgentFactory>(),
            //AgentType.MijnSauna => scope.ServiceProvider.GetRequiredService<MijnSaunaAgentFactory>(),
            //AgentType.PhotoCarousel => scope.ServiceProvider.GetRequiredService<PhotoCarouselAgentFactory>(),
            _ => throw new NotImplementedException()
        };

        return await agent.Create();
    }
}