using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MijnCopilot.Application.Helpers;

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public class MyOrchestrationManager : GroupChatManager
{
    private readonly AgentFactory _agentFactory;

    public MyOrchestrationManager(AgentFactory agentFactory)
    {
        _agentFactory = agentFactory;
    }

    public override ValueTask<GroupChatManagerResult<string>> FilterResults(ChatHistory history, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(new GroupChatManagerResult<string>(history.Last().Content) { Reason = "Custom summary logic." });
    }

    public override async ValueTask<GroupChatManagerResult<string>> SelectNextAgent(ChatHistory history, GroupChatTeam team, CancellationToken cancellationToken = default)
    {
        var agent = _agentFactory.Create(AgentType.Orchestrator);

        var agents = string.Join(",", team.Select(x => $"Name: {x.Key}; Description: {x.Value.Description}"));

        history.AddSystemMessage($"Based on the conversation so far, which agent is best suited to respond next? The available agents are: {agents}. Reply with only the name of the agent!");
        var response = await agent.Chat(history);

        return new GroupChatManagerResult<string>(response);
    }

    public override ValueTask<GroupChatManagerResult<bool>> ShouldRequestUserInput(ChatHistory history, CancellationToken cancellationToken = default)
    {
        var result = new GroupChatManagerResult<bool>(false) { Reason = "No user input required." };
        return ValueTask.FromResult(result);
    }
}

#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.