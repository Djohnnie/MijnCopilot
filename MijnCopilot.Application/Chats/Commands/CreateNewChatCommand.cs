using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CreateNewChatCommandHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<CreateNewChatResponse> Handle(CreateNewChatCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MijnCopilotDbContext>();

        var chatId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        var chat = new Chat
        {
            Id = chatId,
            Title = request.Title,
            StartedOn = timestamp,
            LastActivityOn = timestamp
        };
        dbContext.Chats.Add(chat);

        dbContext.Messages.Add(new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            Content = request.Request,
            PostedOn = timestamp,
            TokensUsed = request.TokensUsed,
            Type = MessageType.User
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateNewChatResponse
        {
            ChatId = chatId
        };
    }
}