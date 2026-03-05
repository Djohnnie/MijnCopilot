using Microsoft.Extensions.AI;

namespace MijnCopilot.Agents.Model;

public class CopilotChatHistory
{
    public List<CopilotChat> Messages { get; private set; } = new List<CopilotChat>();
    public List<DebugChat> Debug { get; private set; } = new List<DebugChat>();
    public string LastAssistantMessage { get; set; }
    public string AgentName { get; set; }
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }

    public int MessageCount => Messages.Count;

    public CopilotChatHistory() { }

    public CopilotChatHistory(string message, CopilotChatRole role)
    {
        Messages.Add(new CopilotChat { Content = message, Role = role });
    }

    public static implicit operator List<ChatMessage>(CopilotChatHistory history)
    {
        var chatHistory = new List<ChatMessage>();

        foreach (var message in history.Messages)
        {
            switch (message.Role)
            {
                case CopilotChatRole.System:
                    chatHistory.Add(new ChatMessage(ChatRole.System, message.Content));
                    break;
                case CopilotChatRole.User:
                    chatHistory.Add(new ChatMessage(ChatRole.User, message.Content));
                    break;
                case CopilotChatRole.Assistant:
                    chatHistory.Add(new ChatMessage(ChatRole.Assistant, message.Content));
                    break;
            }
        }

        return chatHistory;
    }

    public CopilotChatHistory Copy()
    {
        var copy = new CopilotChatHistory();

        foreach (var message in Messages)
        {
            copy.Messages.Add(new CopilotChat
            {
                Role = message.Role,
                Content = message.Content
            });
        }

        return copy;
    }

    public void AddSystemMessage(string message)
    {
        Messages.Add(new CopilotChat
        {
            Role = CopilotChatRole.System,
            Content = message
        });
    }

    public void AddUserMessage(string message)
    {
        Messages.Add(new CopilotChat
        {
            Role = CopilotChatRole.User,
            Content = message
        });
    }

    public void AddAssistantMessage(string message)
    {
        Messages.Add(new CopilotChat
        {
            Role = CopilotChatRole.Assistant,
            Content = message
        });
    }

    public void AddDebug(bool isQuestion, string message, string agentName)
    {
        Debug.Add(new DebugChat
        {
            IsQuestion = isQuestion,
            AgentName = agentName,
            Content = message
        });
    }

    public void Clear()
    {
        Messages.Clear();
    }
}