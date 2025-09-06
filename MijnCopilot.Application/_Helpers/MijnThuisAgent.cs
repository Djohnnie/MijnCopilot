using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers;

public interface IMijnThuisAgentFactory : IAgentFactory
{
    IAgent Create();
}

internal class MijnThuisAgentFactory : AgentFactoryBase, IMijnThuisAgentFactory
{
    protected override string AgentName => "MijnThuisAgent";
    protected override string AgentDescription => "Agent that can handle general user requests";
    protected override string AgentInstruction => "You should answer general questions to the best of your ability";

    public MijnThuisAgentFactory(IConfiguration configuration) : base(configuration) { }
}