using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using MijnCopilot.Application.Helpers.Agents;

namespace MijnCopilot.Application.Helpers;

public enum AgentType
{
    Keyword,
    Orchestrator,
    General,
    MijnThuis,
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

    Task<string> Chat(ChatHistory chatHistory);
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
            AgentType.Orchestrator => scope.ServiceProvider.GetRequiredService<IOrchestratorAgentFactory>(),
            AgentType.General => scope.ServiceProvider.GetRequiredService<IGeneralAgentFactory>(),
            AgentType.MijnThuis => scope.ServiceProvider.GetRequiredService<IMijnThuisAgentFactory>(),
            AgentType.MijnSauna => scope.ServiceProvider.GetRequiredService<IMijnSaunaAgentFactory>(),
            AgentType.PhotoCarousel => scope.ServiceProvider.GetRequiredService<IPhotoCarouselAgentFactory>(),
            _ => throw new NotImplementedException()
        };

        return await agent.Create();
    }
}