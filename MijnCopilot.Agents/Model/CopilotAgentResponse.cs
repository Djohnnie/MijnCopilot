namespace MijnCopilot.Agents.Model;

public class CopilotAgentResponse
{
    public string Response { get; set; } = string.Empty;
    public string AgentName { get; set; } = string.Empty;
    public int InputTokenCount { get; set; } = 0;
    public int OutputTokenCount { get; set; } = 0;
}