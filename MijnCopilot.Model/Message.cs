namespace MijnCopilot.Model;

public class Message
{
    public Guid Id { get; set; }
    public long SysId { get; set; }
    public Chat Chat { get; set; }
    public MessageType Type { get; set; }
    public string Content { get; set; }
    public bool IsReduced { get; set; }
    public int TokensUsed { get; set; }
    public DateTime PostedOn { get; set; }
}