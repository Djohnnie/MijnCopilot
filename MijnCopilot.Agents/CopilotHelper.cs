using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Model;
using MijnCopilot.Agents.Orchestration;

namespace MijnCopilot.Agents;

public interface ICopilotHelper
{
    Task<CopilotKeywordResult> GenerateKeyword(string request);
    Task<CopilotChatHistory> Chat(CopilotChatHistory history);
}

internal class CopilotHelper : ICopilotHelper
{
    private readonly IAgentFactory _agentFactory;
    private readonly IAgentOrchestrationManager _agentOrchestrationManager;
    private readonly IConfiguration _configuration;

    public CopilotHelper(
        IAgentFactory agentFactory,
        IAgentOrchestrationManager agentOrchestrationManager,
        IConfiguration configuration)
    {
        _agentFactory = agentFactory;
        _agentOrchestrationManager = agentOrchestrationManager;
        _configuration = configuration;
    }

    public async Task<CopilotKeywordResult> GenerateKeyword(string request)
    {
        var agent = await _agentFactory.Create(AgentType.Keyword);
        var response = await agent.Chat(new CopilotChatHistory(request, CopilotChatRole.User));

        return new CopilotKeywordResult
        {
            Keyword = response.Response,
            InputTokenCount = response.InputTokenCount,
            OutputTokenCount = response.OutputTokenCount
        };
    }

    public async Task<CopilotChatHistory> Chat(CopilotChatHistory history)
    {
        var response = await _agentOrchestrationManager.Chat(history);

        return response;
    }
}