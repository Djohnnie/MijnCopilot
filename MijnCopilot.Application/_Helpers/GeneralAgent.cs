using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers;

public interface IGeneralAgentFactory : IAgentFactory
{    
}

internal class GeneralAgentFactory : AgentFactoryBase, IGeneralAgentFactory
{
    protected override string AgentName => "GeneralAgent";
    protected override string AgentDescription => "Agent that can handle general user requests";
    protected override string AgentInstruction => "You should answer general questions to the best of your ability";

    public GeneralAgentFactory(IConfiguration configuration) : base(configuration) { }
}