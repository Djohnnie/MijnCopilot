using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers;

public interface IMijnSaunaAgentFactory : IAgentFactory
{
    IAgent Create();
}

internal class MijnSaunaAgentFactory : AgentFactoryBase, IMijnSaunaAgentFactory
{
    protected override string AgentName => "MijnSaunaAgent";
    protected override string AgentDescription => "";
    protected override string AgentInstruction => "";

    public MijnSaunaAgentFactory(IConfiguration configuration) : base(configuration) { }
}