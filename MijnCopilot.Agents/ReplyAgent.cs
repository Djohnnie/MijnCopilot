using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents;

internal class ReplyAgentFactory : AgentFactoryBase
{
    private string _description = "An agent that summarizes all replies to a question into a single reply";
    private string _instructions = @"
You should summarize a reply from a chat history and combine multiple replies in a single sentence or paragraph to capture all information.";

    protected override string AgentName => "QuestionAgent";
    public override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public ReplyAgentFactory(IConfiguration configuration) : base(configuration) { }
}