using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IOrchestratorAgentFactory : IAgentFactory { }

internal class OrchestratorAgentFactory : AgentFactoryBase, IOrchestratorAgentFactory
{
    private string _instructions = "You should forward questions to the correct agent to the best of your ability";

    protected override string AgentName => "OrchestratorAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;

    public OrchestratorAgentFactory(IConfiguration configuration) : base(configuration) { }
}