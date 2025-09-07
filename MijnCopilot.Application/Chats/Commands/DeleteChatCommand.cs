using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.DataAccess;

namespace MijnCopilot.Application.Chats.Commands;

public class DeleteChatCommand : IRequest<Unit>
{
    public Guid ChatId { get; set; }
}

public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand, Unit>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DeleteChatCommandHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Unit> Handle(DeleteChatCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MijnCopilotDbContext>();

        await dbContext.Chats.Where(x => x.Id == request.ChatId).ExecuteUpdateAsync(x => x.SetProperty(p => p.IsArchived, true));

        return Unit.Value;
    }
}