using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MijnCopilot.Application.Helpers;

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public class MyOrchestrationManager : GroupChatManager
{
    private readonly AgentFactory _agentFactory;

    public Action<string, int, int> OrchestrationCallback { get; set; }

    public MyOrchestrationManager(AgentFactory agentFactory)
    {
        _agentFactory = agentFactory;
    }

    public override async ValueTask<GroupChatManagerResult<string>> FilterResults(ChatHistory history, CancellationToken cancellationToken = default)
    {
        var agent = await _agentFactory.Create(AgentType.Reply);
        var response = await agent.Chat(history);
        if (OrchestrationCallback != null) { OrchestrationCallback(agent.Agent.Name, response.InputTokenCount, response.OutputTokenCount); }

        return new GroupChatManagerResult<string>(response.Response) { Reason = "Custom summary logic." };
    }

    public override async ValueTask<GroupChatManagerResult<string>> SelectNextAgent(ChatHistory history, GroupChatTeam team, CancellationToken cancellationToken = default)
    {
        var agent = await _agentFactory.Create(AgentType.Orchestrator);

        var agents = string.Join(",", team.Select(x => $"Name: {x.Key}; Description: {x.Value.Description}"));

        var messages = new ChatMessageContent[history.Count];
        history.CopyTo(messages, 0);
        var chatHistory = new ChatHistory(messages);
        chatHistory.AddSystemMessage($"Based on the conversation so far, which agent is best suited to respond next? The available agents are: {agents}. Reply with only one name of the most prominent agent!");

        var response = await agent.Chat(chatHistory);
        if (OrchestrationCallback != null) { OrchestrationCallback(agent.Agent.Name, response.InputTokenCount, response.OutputTokenCount); }

        return new GroupChatManagerResult<string>(response.Response);
    }

    public override ValueTask<GroupChatManagerResult<bool>> ShouldRequestUserInput(ChatHistory history, CancellationToken cancellationToken = default)
    {
        var result = new GroupChatManagerResult<bool>(false) { Reason = "No user input required." };
        return ValueTask.FromResult(result);
    }

    public override async ValueTask<GroupChatManagerResult<bool>> ShouldTerminate(ChatHistory history, CancellationToken cancellationToken = default)
    {
        var agent = await _agentFactory.Create(AgentType.Question);

        var response = await agent.Chat(history);
        if (OrchestrationCallback != null) { OrchestrationCallback(agent.Agent.Name, response.InputTokenCount, response.OutputTokenCount); }

        return new GroupChatManagerResult<bool>(response.Response.Contains("yes", StringComparison.OrdinalIgnoreCase))
        {
            Reason = "Custom termination logic."
        };
    }
}

#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.