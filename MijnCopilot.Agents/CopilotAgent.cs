using Microsoft.Agents.AI;
using MijnCopilot.Agents.Model;

namespace MijnCopilot.Agents;

internal class CopilotAgent
{
    public AIAgent Agent { get; private set; }

    public CopilotAgent(AIAgent agent)
    {
        Agent = agent;
    }

    public async Task<CopilotAgentResponse> Chat(CopilotChatHistory chatHistory)
    {
        List<Microsoft.Extensions.AI.ChatMessage> chatMessages = chatHistory;

        var agentResponse = await Agent.RunAsync(chatMessages);
        var response = agentResponse.ToString().Replace("**", "");
        var inputTokenCount = (int)(agentResponse.Usage?.InputTokenCount ?? 0);
        var outputTokenCount = (int)(agentResponse.Usage?.OutputTokenCount ?? 0);

        return new CopilotAgentResponse
        {
            Response = response,
            AgentName = Agent.Name,
            InputTokenCount = inputTokenCount,
            OutputTokenCount = outputTokenCount
        };
    }
}