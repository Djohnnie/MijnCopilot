using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisPowerAgentFactory : IAgentFactory { }

internal class MijnThuisPowerAgentFactory : AgentFactoryBase, IMijnThuisPowerAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my power usage via MijnThuis (power usage; peak power usage this month; energy imported and exported today and this month; energy cost today and this month; current energy consumption and injection price)";
    private string _instructions = @"
You should answer questions and receive commands regarding my power usage:
- Power usage and peak power this month
- Energy usage today and this month
- Energy cost today and this month
- Current energy consumption and injection price
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my power usage
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnThuisPowerAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisPowerPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";
    protected override string McpToolPrefix => "mijnthuis_power";

    public MijnThuisPowerAgentFactory(IConfiguration configuration) : base(configuration) { }
}