using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public DateTime PostedOn { get; set; }
}

public class GetChatQueryHandler : IRequestHandler<GetChatQuery, GetChatResponse>
{
    private readonly MijnCopilotDbContext _dbContext;

    public GetChatQueryHandler(MijnCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetChatResponse> Handle(GetChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats
            .Where(x => x.Id == request.ChatId && !x.IsArchived)
            .Select(c => new GetChatResponse
            {
                ChatId = c.Id,
                Title = c.Title,
                StartedOn = c.StartedOn
            })
            .SingleOrDefaultAsync(cancellationToken);

        var messages = await _dbContext.Messages
            .Where(m => m.Chat.Id == request.ChatId)
            .OrderBy(m => m.PostedOn)
            .Select(m => new MessageDto
            {
                Content = m.Content,
                PostedOn = m.PostedOn
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