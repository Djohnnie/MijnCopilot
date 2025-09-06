using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IKeywordAgentFactory : IAgentFactory
{
}

internal class KeywordAgentFactory : AgentFactoryBase, IKeywordAgentFactory
{
    protected override string AgentName => "KeywordAgent";
    protected override string AgentDescription => "Agent that summarizes a user request to one or two keywords";
    protected override string AgentInstruction => "You should summarize a user request to one or two keywords and start each keyword with a capital letter.";

    public KeywordAgentFactory(IConfiguration configuration) : base(configuration) { }
}