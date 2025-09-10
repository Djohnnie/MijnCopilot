using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using ModelContextProtocol.Client;
using OpenAI.Chat;
using System.Diagnostics;
using System.Text;

namespace MijnCopilot.Application.Helpers;

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

internal abstract class AgentFactoryBase
{
    private readonly IConfiguration _configuration;
    private IMcpClient _mcpClient;

    protected virtual string AgentName => "AGENT_NAME";
    protected virtual string AgentDescription => "AGENT_DESCRIPTION";
    protected virtual string AgentInstruction => "AGENT_INSTRUCTIONS";
    protected virtual bool HasPlugin => false;
    protected virtual string PluginName => "PLUGIN_NAME";
    protected virtual string McpName => "MCP_NAME";
    protected virtual string McpEndpointConfig => "MCP_ENDPOINT";
    protected virtual string McpToolPrefix => string.Empty;

    protected AgentFactoryBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IAgent> Create()
    {
        return new MyAgent(
            new ChatCompletionAgent
            {
                Name = AgentName,
                Description = AgentDescription,
                Instructions = AgentInstruction,
                Kernel = await InitializeKernel(),
                Arguments = new KernelArguments(new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                })
            });
    }

    protected async Task<Kernel> InitializeKernel()
    {
        var deployment = _configuration["AZUREOPENAI_DEPLOYMENT"];
        var endpoint = _configuration["AZUREOPENAI_ENDPOINT"];
        var key = _configuration["AZUREOPENAI_KEY"];

        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(deployment, endpoint, key);
        builder.Services.AddSingleton(_configuration);

        if (HasPlugin)
        {
            var tools = await InitializeTools();
            builder.Plugins.AddFromFunctions(PluginName, tools.Select(f => f.AsKernelFunction()));
        }

        return builder.Build();
    }

    protected virtual async Task InitializeMcpClient()
    {
        var endpoint = _configuration.GetValue<string>(McpEndpointConfig);

        _mcpClient = await McpClientFactory.CreateAsync(
            new SseClientTransport(new()
            {
                Name = McpName,
                Endpoint = new Uri(endpoint)
            }));
    }

    protected virtual async ValueTask<IList<McpClientTool>> InitializeTools()
    {
        await InitializeMcpClient();
        var tools = await _mcpClient.ListToolsAsync();
        return tools.Where(x => string.IsNullOrEmpty(McpToolPrefix) || x.Name.StartsWith(McpToolPrefix)).ToList();
    }
}

internal class MyAgent : IAgent
{
    public ChatCompletionAgent Agent { get; private set; }

    public MyAgent(ChatCompletionAgent agent)
    {
        Agent = agent;
    }

    public async Task<MyAgentResponse> Chat(ChatHistory chatHistory)
    {
        var responseBuilder = new StringBuilder();
        var inputTokenCount = 0;
        var outputTokenCount = 0;

        await foreach (var message in Agent.InvokeAsync(chatHistory))
        {
            responseBuilder.Append(message.Message.Content);

            if (message.Message.Metadata != null && message.Message.Metadata.ContainsKey("Usage"))
            {
                var usage = message.Message.Metadata["Usage"] as ChatTokenUsage;
                if (usage != null)
                {
                    inputTokenCount += usage.InputTokenCount;
                    outputTokenCount += usage.OutputTokenCount;
                }
            }
        }

        var response = responseBuilder.ToString();

        return new MyAgentResponse
        {
            Response = responseBuilder.ToString(),
            InputTokenCount = inputTokenCount,
            OutputTokenCount = outputTokenCount
        };
    }
}

public class MyAgentResponse
{
    public string Response { get; set; } = string.Empty;
    public int InputTokenCount { get; set; } = 0;
    public int OutputTokenCount { get; set; } = 0;
}

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.