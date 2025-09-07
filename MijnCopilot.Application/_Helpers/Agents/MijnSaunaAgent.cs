using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IMijnSaunaAgentFactory : IAgentFactory { }

internal class MijnSaunaAgentFactory : AgentFactoryBase, IMijnSaunaAgentFactory
{
    private string _instructions = "You should answer questions regarding my sauna (temperature inside and outside, sauna or infrared and status) to the best of your ability";

    protected override string AgentName => "MijnSaunaAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;

    public MijnSaunaAgentFactory(IConfiguration configuration) : base(configuration) { }
}