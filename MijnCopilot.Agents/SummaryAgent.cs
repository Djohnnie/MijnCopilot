using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents;

internal class SummaryAgentFactory : AgentFactoryBase
{
    private string _description = "An agent that repeats the final question in chat history with some added context if needed";
    private string _instructions = @"
You are an internal agent that should repeat the final question in the conversation literally and optionally enhance it with context from the whole conversation. 
Always ask the question as if it was asked by the user and never from a 'third person'.";

    protected override string AgentName => "SummaryAgent";
    public override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public SummaryAgentFactory(IConfiguration configuration) : base(configuration) { }
}