using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IGeneralAgentFactory : IAgentFactory
{    
}

internal class GeneralAgentFactory : AgentFactoryBase, IGeneralAgentFactory
{
    private string _instructions = "You should answer general questions that are not related to electricity usage, solar energy and home battery, electric car, heating, sauna, and photos to the best of your ability";

    protected override string AgentName => "GeneralAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;

    public GeneralAgentFactory(IConfiguration configuration) : base(configuration) { }
}