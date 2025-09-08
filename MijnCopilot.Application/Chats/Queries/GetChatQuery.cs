using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.DataAccess;

namespace MijnCopilot.Application.Chats.Queries;

public class GetChatQuery : IRequest<GetChatResponse>
{
    public Guid ChatId { get; set; }
}

public class GetChatResponse
{
    public Guid ChatId { get; set; }
    public string Title { get; set; }
    public DateTime StartedOn { get; set; }

    public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
}

public class MessageDto
{
    public string Content { get; set; }
    public string AgentName { get; set; }
    public ChatRole Role { get; set; }
    public int TokensUsed { get; set; }
    public DateTime PostedOn { get; set; }
}

public enum ChatRole
{
    User,
    Assistant,
    Reduced
}

public class GetChatQueryHandler : IRequestHandler<GetChatQuery, GetChatResponse>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GetChatQueryHandler(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<GetChatResponse> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MijnCopilotDbContext>();

        var chat = await dbContext.Chats
            .Where(x => x.Id == request.ChatId && !x.IsArchived)
            .Select(c => new GetChatResponse
            {
                ChatId = c.Id,
                Title = c.Title,
                StartedOn = c.StartedOn
            })
            .SingleOrDefaultAsync(cancellationToken);

        var messages = await dbContext.Messages
            .Where(m => m.Chat.Id == request.ChatId)
            .OrderBy(m => m.PostedOn)
            .Select(m => new MessageDto
            {
                Content = m.Content,
                AgentName = m.AgentName,
                Role = m.Type == Model.MessageType.Assistant ? ChatRole.Assistant : ChatRole.User,
                PostedOn = m.PostedOn,
                TokensUsed = m.TokensUsed,
            })
            .ToListAsync(cancellationToken);

        return new GetChatResponse
        {
            ChatId = chat?.ChatId ?? Guid.Empty,
            Title = chat?.Title ?? string.Empty,
            StartedOn = chat?.StartedOn ?? DateTime.MinValue,
            Messages = messages
        };
    }
}