using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface ISummaryAgentFactory : IAgentFactory
{
}

internal class SummaryAgentFactory : AgentFactoryBase, ISummaryAgentFactory
{
    private string _instructions = "...";

    protected override string AgentName => "SummaryAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;

    public SummaryAgentFactory(IConfiguration configuration) : base(configuration) { }
}