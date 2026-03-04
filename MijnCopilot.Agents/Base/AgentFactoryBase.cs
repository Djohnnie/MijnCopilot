using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Client;
using OpenAI.Chat;
using System.ClientModel;

namespace MijnCopilot.Agents.Base;

internal abstract class AgentFactoryBase
{
    private readonly IConfiguration _configuration;
    private McpClient _mcpClient;

    protected virtual string AgentName => "AGENT_NAME";
    public virtual string AgentDescription => "AGENT_DESCRIPTION";
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

    public async Task<CopilotAgent> Create()
    {
        var deployment = _configuration.GetValue<string>("AZUREOPENAI_DEPLOYMENT");
        var endpoint = _configuration.GetValue<string>("AZUREOPENAI_ENDPOINT");
        var key = _configuration.GetValue<string>("AZUREOPENAI_KEY");

        var tools = new List<AITool>();

        if (HasPlugin)
        {
            var mcpTools = await InitializeTools();
            tools.AddRange(mcpTools.Cast<AITool>());
        }

        var client = new AzureOpenAIClient(new Uri(endpoint), new ApiKeyCredential(key));
        var chatClient = client.GetChatClient(deployment);
        var agentClient = chatClient.AsAIAgent(
            name: AgentName, description: AgentDescription, instructions: AgentInstruction,
            tools: tools/*, services: serviceCollection.BuildServiceProvider()*/);

        return new CopilotAgent(agentClient);
    }

    protected virtual async Task InitializeMcpClient()
    {
        var endpoint = _configuration.GetValue<string>(McpEndpointConfig);

        _mcpClient = await McpClient.CreateAsync(
            new HttpClientTransport(new()
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