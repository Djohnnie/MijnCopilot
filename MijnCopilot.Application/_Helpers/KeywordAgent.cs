using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using OpenAI.Chat;
using System.Text;

namespace MijnCopilot.Application.Helpers;

public interface IKeywordAgent
{
    Task<string> GenerateKeyword(Kernel kernel, string request);
}

public class KeywordAgent : IKeywordAgent
{
    public async Task<string> GenerateKeyword(Kernel kernel, string request)
    {
        var chatCompletionAgent = new ChatCompletionAgent
        {
            Name = "KeywordAgent",
            Description = "Agent that summarizes a user request to one or two keywords",
            Instructions = "You should summarize a user request to one or two keywords",
            Kernel = kernel
        };

        var agentThread = new ChatHistoryAgentThread();
        agentThread.ChatHistory.AddUserMessage(request);

        var messageBuilder = new StringBuilder();
        await foreach (var response in chatCompletionAgent.InvokeAsync(agentThread))
        {
            if (response.Message.Metadata.ContainsKey("Usage"))
            {
                var usage = response.Message.Metadata["Usage"] as ChatTokenUsage;
                if (usage != null)
                {
                    //chatHistory.InputTokenCount = usage.InputTokenCount;
                    //chatHistory.OutputTokenCount = usage.OutputTokenCount;
                }
            }
            messageBuilder.Append(response.Message.Content);
        }

        return messageBuilder.ToString();
    }
}