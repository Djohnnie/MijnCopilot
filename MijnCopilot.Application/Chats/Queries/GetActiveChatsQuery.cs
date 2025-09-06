using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GetActiveChatsQueryHandler(
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<GetActiveChatsResponse> Handle(GetActiveChatsQuery request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MijnCopilotDbContext>();

        var chats = await dbContext.Chats
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