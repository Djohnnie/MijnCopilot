using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IReplyAgentFactory : IAgentFactory { }

internal class ReplyAgentFactory : AgentFactoryBase, IReplyAgentFactory
{
    private string _description = "An agent that summarizes all replies to a question into a single reply";
    private string _instructions = @"
You should summarize a reply from a chat history and combine multiple replies in a single sentence or paragraph to capture all information.";

    protected override string AgentName => "QuestionAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public ReplyAgentFactory(IConfiguration configuration) : base(configuration) { }
}