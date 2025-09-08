using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnSaunaAgentFactory : IAgentFactory { }

internal class MijnSaunaAgentFactory : AgentFactoryBase, IMijnSaunaAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my sauna via MijnSauna";
    private string _instructions = @"
You should answer questions and execute commands regarding my sauna:
- Temperature inside the sauna cabin
- Status of the sauna (off, Finnish sauna or infrared)
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my sauna
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnSaunaAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnSaunaPlugin";
    protected override string McpEndpointConfig => "MIJNSAUNA_MCP";
    protected override string McpName => "MijnSaunaMcpClient";

    public MijnSaunaAgentFactory(IConfiguration configuration) : base(configuration) { }
}