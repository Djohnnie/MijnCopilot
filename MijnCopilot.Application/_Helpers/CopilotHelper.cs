using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Orchestration.GroupChat;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Diagnostics;

#pragma warning disable SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace MijnCopilot.Application.Helpers;

// https://learn.microsoft.com/en-us/semantic-kernel/frameworks/agent/agent-orchestration/group-chat?pivots=programming-language-csharp

public interface ICopilotHelper
{
    Task<string> GenerateKeyword(string request);
    Task<string> Chat(string request);
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

    public async Task<string> Chat(string request)
    {
        var generalAgent = await _agentFactory.Create(AgentType.General);
        var mijnThuisAgent = await _agentFactory.Create(AgentType.MijnThuis);
        var mijnSaunaAgent = await _agentFactory.Create(AgentType.MijnSauna);
        var photoCarouselAgent = await _agentFactory.Create(AgentType.PhotoCarousel);

        var manager = new MyOrchestrationManager(_agentFactory) { MaximumInvocationCount = 1 };
        var orchestration = new GroupChatOrchestration(manager, generalAgent.Agent, mijnThuisAgent.Agent, mijnSaunaAgent.Agent, photoCarouselAgent.Agent)
        {
            ResponseCallback = responseCallback,
        };

        var runtime = new InProcessRuntime();
        await runtime.StartAsync();

        var result = await orchestration.InvokeAsync(request, runtime);
        var response = await result.GetValueAsync();

        await runtime.RunUntilIdleAsync();

        return response;
    }

    private async ValueTask responseCallback(ChatMessageContent response)
    {
        _history.Add(response);
        Debug.WriteLine($"Response from {response.AuthorName}: {response.Content}");
    }
}

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.