using Microsoft.SemanticKernel.ChatCompletion;

namespace MijnCopilot.Application.Helpers;

public class MyChatHistory
{
    public List<MyChat> Messages { get; private set; } = new List<MyChat>();
    public string LastAssistantMessage { get; set; }
    public string AgentName { get; set; }
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }

    public int MessageCount => Messages.Count;

    public static implicit operator ChatHistory(MyChatHistory history)
    {
        var chatHistory = new ChatHistory();

        foreach (var message in history.Messages)
        {
            switch (message.Role)
            {
                case MyChatRole.System:
                    chatHistory.AddSystemMessage(message.Content);
                    break;
                case MyChatRole.User:
                    chatHistory.AddUserMessage(message.Content);
                    break;
                case MyChatRole.Assistant:
                    chatHistory.AddAssistantMessage(message.Content);
                    break;
            }
        }

        return chatHistory;
    }

    public MyChatHistory Copy()
    {
        var copy = new MyChatHistory();

        foreach (var message in Messages)
        {
            copy.Messages.Add(new MyChat
            {
                Role = message.Role,
                Content = message.Content
            });
        }

        return copy;
    }

    public void AddSystemMessage(string message)
    {
        Messages.Add(new MyChat
        {
            Role = MyChatRole.System,
            Content = message
        });
    }

    public void AddUserMessage(string message)
    {
        Messages.Add(new MyChat
        {
            Role = MyChatRole.User,
            Content = message
        });
    }

    public void AddAssistantMessage(string message)
    {
        Messages.Add(new MyChat
        {
            Role = MyChatRole.Assistant,
            Content = message
        });
    }

    public void Clear()
    {
        Messages.Clear();
    }
}

public class MyChat
{
    public MyChatRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
}

public enum MyChatRole
{
    System,
    User,
    Assistant
}