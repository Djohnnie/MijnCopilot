using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents;

internal class SummaryAgentFactory : AgentFactoryBase
{
    private string _description = "An agent that summarizes questions and commands in chat history";
    private string _instructions = @"
You are an internal agent that should repeat the final question in the conversation. 
If the question is not clear, you can use the whole conversation as context and make the final question more clear.";

    protected override string AgentName => "SummaryAgent";
    public override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public SummaryAgentFactory(IConfiguration configuration) : base(configuration) { }
}