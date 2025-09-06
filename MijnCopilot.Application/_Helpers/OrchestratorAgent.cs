using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers;

public interface IOrchestratorAgentFactory : IAgentFactory
{
    IAgent Create();
}

internal class OrchestratorAgentFactory : AgentFactoryBase, IOrchestratorAgentFactory
{
    protected override string AgentName => "MijnThuisAgent";
    protected override string AgentDescription => "Agent that can handle general user requests";
    protected override string AgentInstruction => "You should answer general questions to the best of your ability";

    public OrchestratorAgentFactory(IConfiguration configuration) : base(configuration) { }
}