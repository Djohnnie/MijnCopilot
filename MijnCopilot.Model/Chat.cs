namespace MijnCopilot.Model;

public class Chat
{
    public Guid Id { get; set; }
    public long SysId { get; set; }
    public string Title { get; set; }
    public bool IsArchived { get; set; }
    public DateTime StartedOn { get; set; }
    public DateTime LastActivityOn { get; set; }
}