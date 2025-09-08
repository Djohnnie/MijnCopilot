using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI.Chat;
using System.Diagnostics;

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace MijnCopilot.Application.Helpers;

// https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-orchestration/group-chat?pivots=programming-language-csharp

public interface ICopilotHelper
{
    Task<string> GenerateKeyword(string request);
    Task<MyChatHistory> Chat(MyChatHistory history);
}

public class CopilotHelper : ICopilotHelper
{
    private readonly AgentFactory _agentFactory;
    private readonly ChatHistory _history = new();

    public CopilotHelper(
        AgentFactory agentFactory,
        IConfiguration configuration)
    {
        _agentFactory = agentFactory;
    }

    public async Task<string> GenerateKeyword(string request)
    {
        var agent = await _agentFactory.Create(AgentType.Keyword);
        return await agent.Chat(new ChatHistory(request, AuthorRole.User));
    }

    public async Task<MyChatHistory> Chat(MyChatHistory history)
    {
        // Start by clearing the internal orchestrator history for this session.
        _history.Clear();

        // Use the agent factory to create the agents needed for the orchestration.
        var generalAgent = await _agentFactory.Create(AgentType.General);
        var mijnThuisPowerAgent = await _agentFactory.Create(AgentType.MijnThuisPower);
        var mijnThuisSolarAgent = await _agentFactory.Create(AgentType.MijnThuisSolar);
        var mijnThuisCarAgent = await _agentFactory.Create(AgentType.MijnThuisCar);
        var mijnThuisHeatingAgent = await _agentFactory.Create(AgentType.MijnThuisHeating);
        var mijnThuisSmartLockAgent = await _agentFactory.Create(AgentType.MijnThuisSmartLock);
        var mijnSaunaAgent = await _agentFactory.Create(AgentType.MijnSauna);
        var photoCarouselAgent = await _agentFactory.Create(AgentType.PhotoCarousel);

        // Create the orchestration manager and the orchestration itself.
        var manager = new MyOrchestrationManager(_agentFactory) { MaximumInvocationCount = 1 };
        var orchestration = new GroupChatOrchestration(manager, generalAgent.Agent, mijnThuisPowerAgent.Agent, mijnThuisSolarAgent.Agent, mijnThuisCarAgent.Agent, mijnThuisHeatingAgent.Agent, mijnThuisSmartLockAgent.Agent, mijnSaunaAgent.Agent, photoCarouselAgent.Agent)
        {
            ResponseCallback = responseCallback,
        };

        // Summarize the chat history to provide a clear question for the orchestration.
        var summary = await SummarizeChatHistory(history);

        // Start an in-process runtime to run the orchestration.
        var runtime = new InProcessRuntime();
        await runtime.StartAsync();

        // Invoke the orchestration with the summarized input and retrieve the response.
        var result = await orchestration.InvokeAsync(summary, runtime);
        var response = await result.GetValueAsync();

        // Ensure all asynchronous operations are completed.
        await runtime.RunUntilIdleAsync();

        foreach (var entry in _history)
        {
            var inputTokenCount = 0;
            var outputTokenCount = 0;

            if (entry.Metadata != null && entry.Metadata.ContainsKey("Usage"))
            {
                var usage = entry.Metadata["Usage"] as ChatTokenUsage;
                if (usage != null)
                {
                    inputTokenCount = usage.InputTokenCount;
                    outputTokenCount = usage.OutputTokenCount;
                }
            }

            Debug.WriteLine($"{entry.AuthorName}: {entry.Content} ({inputTokenCount}/{outputTokenCount})");
        }

        // Add the response to a copy of the provided history and return it.
        var newHistory = history.Copy();
        newHistory.AddAssistantMessage(response);
        newHistory.LastAssistantMessage = response;

        return newHistory;
    }

    private async Task<string> SummarizeChatHistory(MyChatHistory history)
    {
        var summaryAgent = await _agentFactory.Create(AgentType.Summary);
        return await summaryAgent.Chat(history);
    }

    private async ValueTask responseCallback(Microsoft.SemanticKernel.ChatMessageContent response)
    {
        _history.Add(response);
        //Debug.WriteLine($"Response from {response.AuthorName}: {response.Content}");
    }
}

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.