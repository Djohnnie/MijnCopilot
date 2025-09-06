using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text;

namespace MijnCopilot.Application.Helpers;

internal abstract class AgentFactoryBase
{
    private readonly IConfiguration _configuration;

    protected virtual string AgentName => "AGENT_NAME";
    protected virtual string AgentDescription => "AGENT_DESCRIPTION";
    protected virtual string AgentInstruction => "AGENT_INSTRUCTIONS";

    protected AgentFactoryBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IAgent Create()
    {
        return new MyAgent(
            new ChatCompletionAgent
            {
                Name = AgentName,
                Description = AgentDescription,
                Instructions = AgentInstruction,
                Kernel = InitializeKernel(),
                Arguments = new KernelArguments(new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                })
            });
    }

    protected Kernel InitializeKernel()
    {
        var deployment = _configuration["AZUREOPENAI_DEPLOYMENT"];
        var endpoint = _configuration["AZUREOPENAI_ENDPOINT"];
        var key = _configuration["AZUREOPENAI_KEY"];

        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(deployment, endpoint, key);
        builder.Services.AddSingleton(_configuration);
        //builder.Plugins.AddFromType<CopilotFunctions>();

        return builder.Build();
    }
}

internal class MyAgent : IAgent
{
    public ChatCompletionAgent Agent { get; private set; }

    public MyAgent(ChatCompletionAgent agent)
    {
        Agent = agent;
    }

    public async Task<string> Chat(ChatHistory chatHistory)
    {
        var responseBuilder = new StringBuilder();
        await foreach (var message in Agent.InvokeAsync(chatHistory))
        {
            responseBuilder.Append(message.Message.Content);
        }

        return responseBuilder.ToString();
    }
}