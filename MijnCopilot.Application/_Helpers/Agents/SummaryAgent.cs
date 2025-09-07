using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface ISummaryAgentFactory : IAgentFactory { }

internal class SummaryAgentFactory : AgentFactoryBase, ISummaryAgentFactory
{
    private string _description = "...";
    private string _instructions = "You are a summary agent. Your task is to generate concise and informative summaries of the provided content. Focus on capturing the main points and key details while maintaining clarity and coherence. Ensure that the summary is easy to understand and free of unnecessary jargon or complex language. Aim to provide a brief overview that highlights the essential information without losing the context of the original content.";

    protected override string AgentName => "SummaryAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;

    public SummaryAgentFactory(IConfiguration configuration) : base(configuration) { }
}