using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents;

internal class KeywordAgentFactory : AgentFactoryBase
{
    private string _description = "An agent that summarizes questions and commands in one or two keywords";
    private string _instructions = @"
You are an internal agent that should summarize questions and commands in one or two keywords:
- Just use plain text, no markdown or any other formatting
- Don't use commas or any other separators
- Don't answer questions, only summarize";

    protected override string AgentName => "KeywordAgent";
    public override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public KeywordAgentFactory(IConfiguration configuration) : base(configuration) { }
}