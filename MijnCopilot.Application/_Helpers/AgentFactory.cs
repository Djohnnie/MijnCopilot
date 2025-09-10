using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using MijnCopilot.Application.Helpers.Agents;

namespace MijnCopilot.Application.Helpers;

public enum AgentType
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

public interface IAgentFactory
{
    Task<IAgent> Create();
}

public interface IAgent
{
    ChatCompletionAgent Agent { get; }

    Task<MyAgentResponse> Chat(ChatHistory chatHistory);
}

public class AgentFactory
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AgentFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IAgent> Create(AgentType type)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        IAgentFactory agent = type switch
        {
            AgentType.Keyword => scope.ServiceProvider.GetRequiredService<IKeywordAgentFactory>(),
            AgentType.Summary => scope.ServiceProvider.GetRequiredService<ISummaryAgentFactory>(),
            AgentType.Orchestrator => scope.ServiceProvider.GetRequiredService<IOrchestratorAgentFactory>(),
            AgentType.Question => scope.ServiceProvider.GetRequiredService<IQuestionAgentFactory>(),
            AgentType.Reply => scope.ServiceProvider.GetRequiredService<IReplyAgentFactory>(),
            AgentType.General => scope.ServiceProvider.GetRequiredService<IGeneralAgentFactory>(),
            AgentType.MijnThuisPower => scope.ServiceProvider.GetRequiredService<IMijnThuisPowerAgentFactory>(),
            AgentType.MijnThuisSolar => scope.ServiceProvider.GetRequiredService<IMijnThuisSolarAgentFactory>(),
            AgentType.MijnThuisCar => scope.ServiceProvider.GetRequiredService<IMijnThuisCarAgentFactory>(),
            AgentType.MijnThuisHeating => scope.ServiceProvider.GetRequiredService<IMijnThuisHeatingAgentFactory>(),
            AgentType.MijnThuisSmartLock => scope.ServiceProvider.GetRequiredService<IMijnThuisSmartLockAgentFactory>(),
            AgentType.MijnSauna => scope.ServiceProvider.GetRequiredService<IMijnSaunaAgentFactory>(),
            AgentType.PhotoCarousel => scope.ServiceProvider.GetRequiredService<IPhotoCarouselAgentFactory>(),
            _ => throw new NotImplementedException()
        };

        return await agent.Create();
    }
}