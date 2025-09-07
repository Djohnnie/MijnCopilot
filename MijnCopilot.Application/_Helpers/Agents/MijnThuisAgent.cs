using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisAgentFactory : IAgentFactory { }

internal class MijnThuisAgentFactory : AgentFactoryBase, IMijnThuisAgentFactory
{
    private string _instructions = "You should answer questions regarding my smart home (electricity usage, solar power and home battery, heating, electric car) to the best of your ability";

    protected override string AgentName => "MijnThuisAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";

    public MijnThuisAgentFactory(IConfiguration configuration) : base(configuration) { }
}