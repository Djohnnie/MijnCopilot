using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisSmartLockAgentFactory : IAgentFactory { }

internal class MijnThuisSmartLockAgentFactory : AgentFactoryBase, IMijnThuisSmartLockAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my smart lock via MijnThuis";
    private string _instructions = @"
You should answer questions and receive commands regarding my smart lock:
- Current state of the lock (locked/unlocked)
- Current state of the door (open/closed)
- Battery percentage of the smart lock
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my smart lock
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnThuisSmartLockAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisSmartLockPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";
    protected override string McpToolPrefix => "mijnthuis_heating";

    public MijnThuisSmartLockAgentFactory(IConfiguration configuration) : base(configuration) { }
}