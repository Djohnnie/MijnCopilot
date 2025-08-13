using MediatR;
using MijnCopilot.DataAccess;
using MijnCopilot.Model;

namespace MijnCopilot.Application.Chats.Commands;

public class CreateNewChatCommand : IRequest<CreateNewChatResponse>
{
    public string Title { get; set; }
    public string Request { get; set; }
    public int TokensUsed { get; set; }
}

public class CreateNewChatResponse
{
    public Guid ChatId { get; set; }
}

public class CreateNewChatCommandHandler : IRequestHandler<CreateNewChatCommand, CreateNewChatResponse>
{
    private readonly MijnCopilotDbContext _dbContext;

    public CreateNewChatCommandHandler(MijnCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateNewChatResponse> Handle(CreateNewChatCommand request, CancellationToken cancellationToken)
    {
        var chatId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        var chat = new Chat
        {
            Id = chatId,
            Title = request.Title,
            StartedOn = timestamp,
            LastActivityOn = timestamp
        };
        _dbContext.Chats.Add(chat);

        _dbContext.Messages.Add(new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            Content = request.Request,
            PostedOn = timestamp,
            TokensUsed = request.TokensUsed,
            Type = MessageType.User
        });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateNewChatResponse
        {
            ChatId = chatId
        };
    }
}