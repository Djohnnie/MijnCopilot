using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MijnCopilot.Application.Helpers;
using MijnCopilot.DataAccess;
using MijnCopilot.Model;

namespace MijnCopilot.Application.Agents.Commands;

public class ChatCommand : IRequest<ChatResponse>
{
    public Guid ChatId { get; set; }
    public string Request { get; set; }
    public bool IgnoreRequest { get; set; }
}

public class ChatResponse
{
    public string Response { get; set; }
}

public class ChatCommandHandler : IRequestHandler<ChatCommand, ChatResponse>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ICopilotHelper _copilotHelper;

    public ChatCommandHandler(
        IServiceScopeFactory serviceScopeFactory,
        ICopilotHelper copilotHelper)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _copilotHelper = copilotHelper;
    }
    public async Task<ChatResponse> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MijnCopilotDbContext>();

        var messages = await dbContext.Messages
            .Where(m => m.Chat.Id == request.ChatId)
            .OrderBy(m => m.PostedOn)
            .ToListAsync(cancellationToken);

        var chatHistory = new MyChatHistory();
        foreach (var message in messages)
        {
            switch (message.Type)
            {
                case MessageType.User:
                    chatHistory.AddUserMessage(message.Content);
                    break;
                case MessageType.Assistant:
                    chatHistory.AddAssistantMessage(message.Content);
                    break;
            }
        }

        if (!request.IgnoreRequest)
        {
            chatHistory.AddUserMessage(request.Request);
        }

        var copilotResponse = await _copilotHelper.Chat(chatHistory);

        var chat = await dbContext.Chats.SingleOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

        if (!request.IgnoreRequest)
        {
            dbContext.Messages.Add(new Message
            {
                Id = Guid.NewGuid(),
                Chat = chat,
                Content = request.Request,
                AgentName = string.Empty,
                PostedOn = DateTime.UtcNow,
                TokensUsed = copilotResponse.InputTokenCount,
                Type = MessageType.User
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        dbContext.Messages.Add(new Message
        {
            Id = Guid.NewGuid(),
            Chat = chat,
            Content = copilotResponse.LastAssistantMessage,
            AgentName = GetAgentName(copilotResponse.AgentName),
            PostedOn = DateTime.UtcNow,
            TokensUsed = copilotResponse.OutputTokenCount,
            Type = MessageType.Assistant
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new ChatResponse
        {
            Response = copilotResponse.LastAssistantMessage
        };
    }

    public static string GetAgentName(string internalName)
    {
        return internalName switch
        {
            "GeneralAgent" => "General",
            "MijnThuisPowerAgent" => "MijnThuis - Power",
            "MijnThuisSolarAgent" => "MijnThuis - Solar",
            "MijnThuisCarAgent" => "MijnThuis - Car",
            "MijnThuisHeatingAgent" => "MijnThuis - Heating",
            "MijnThuisSmartLockAgent" => "MijnThuis - SmartLock",
            "MijnSaunaAgent" => "MijnSauna",
            "PhotoCarouselAgent" => "PhotoCarousel",
            _ => internalName
        };
    }
}