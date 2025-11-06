using Microsoft.Extensions.Configuration;
using MijnCopilot.Agents.Base;

namespace MijnCopilot.Agents;

internal class QuestionAgentFactory : AgentFactoryBase
{
    private string _description = "An agent that checks if all questions are answered";
    private string _instructions = @"
You should check if all questions in the conversation have been answered and only reply with YES or NO";

    protected override string AgentName => "QuestionAgent";
    public override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public QuestionAgentFactory(IConfiguration configuration) : base(configuration) { }
}