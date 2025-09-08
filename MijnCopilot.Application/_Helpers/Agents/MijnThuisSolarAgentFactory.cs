using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisSolarAgentFactory : IAgentFactory { }

internal class MijnThuisSolarAgentFactory : AgentFactoryBase, IMijnThuisSolarAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my solar installation via MijnThuis (current solar production; current battery power; current grid power; solar/home battery charge percentage and health; solar energy production today and this month; solar energy forecast today and tomorrow)";
    private string _instructions = @"
You should answer questions and receive commands regarding my solar installation:
- Solar power production, battery power and grid power
- Solar battery charge state and health
- Solar energy production today and this month
- Solar forecast for today and tomorrow
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my solar installation
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnThuisSolarAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisSolarPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";
    protected override string McpToolPrefix => "mijnthuis_solar";

    public MijnThuisSolarAgentFactory(IConfiguration configuration) : base(configuration) { }
}