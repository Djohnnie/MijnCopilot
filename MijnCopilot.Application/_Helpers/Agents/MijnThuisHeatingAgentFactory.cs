using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnThuisHeatingAgentFactory : IAgentFactory { }

internal class MijnThuisHeatingAgentFactory : AgentFactoryBase, IMijnThuisHeatingAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my heating installation via MijnThuis (current temperature in my home and outside; current setpoint temperature and next setpoint temperature; next setpoint scheduled time)";
    private string _instructions = @"
You should answer questions and receive commands regarding my heating installation:
- Current temperature in my home
- Current temperature outside
- Current setpoint temperature and next setpoint temperature
- Next setpoint change time
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my heating installation
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "MijnThuisHeatingAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "MijnThuisHeatingPlugin";
    protected override string McpEndpointConfig => "MIJNTHUIS_MCP";
    protected override string McpName => "MijnThuisMcpClient";
    protected override string McpToolPrefix => "mijnthuis_heating";

    public MijnThuisHeatingAgentFactory(IConfiguration configuration) : base(configuration) { }
}