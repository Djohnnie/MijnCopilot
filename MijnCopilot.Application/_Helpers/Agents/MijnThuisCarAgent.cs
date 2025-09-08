using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisCarAgentFactory : IAgentFactory { }

internal class MijnThuisCarAgentFactory : AgentFactoryBase, IMijnThuisCarAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my electric car via MijnThuis";
    private string _instructions = @"
You should answer questions and receive commands regarding my electric car:
- Current location of my car
- Car charge state
- Start or stop charging
- Locking and unlocking the car
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my electric car
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnThuisCarAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisCarPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";
    protected override string McpToolPrefix => "mijnthuis_car";

    public MijnThuisCarAgentFactory(IConfiguration configuration) : base(configuration) { }
}