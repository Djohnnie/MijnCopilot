using MediatR;
using Microsoft.EntityFrameworkCore;
using MijnCopilot.DataAccess;

namespace MijnCopilot.Application.Chats.Queries;

public class GetActiveChatsQuery : IRequest<GetActiveChatsResponse>
{
}

public class GetActiveChatsResponse
{
    public List<ChatDto> Chats { get; set; } = new List<ChatDto>();
}

public class ChatDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}

public class GetActiveChatsQueryHandler : IRequestHandler<GetActiveChatsQuery, GetActiveChatsResponse>
{
    private readonly MijnCopilotDbContext _dbContext;

    public GetActiveChatsQueryHandler(
        MijnCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetActiveChatsResponse> Handle(GetActiveChatsQuery request, CancellationToken cancellationToken)
    {
        var chats = await _dbContext.Chats
            .Where(x => !x.IsArchived)
            .OrderByDescending(x => x.StartedOn)
            .Select(c => new ChatDto
            {
                Id = c.Id,
                Title = c.Title
            })
            .ToListAsync(cancellationToken);

        return new GetActiveChatsResponse
        {
            Chats = chats
        };
    }
}