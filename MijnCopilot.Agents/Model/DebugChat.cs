namespace MijnCopilot.Agents.Model;

public class DebugChat
{
    public bool IsQuestion { get; set; }
    public string AgentName { get; set; }
    public string Content { get; set; } = string.Empty;
}