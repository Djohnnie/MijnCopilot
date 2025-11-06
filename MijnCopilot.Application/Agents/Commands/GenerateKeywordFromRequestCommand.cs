using MediatR;
using MijnCopilot.Agents;

namespace MijnCopilot.Application.Agents.Commands;

public class GenerateKeywordFromRequestCommand : IRequest<GenerateKeywordFromRequestResponse>
{
    public string Request { get; set; }
}

public class GenerateKeywordFromRequestResponse
{
    public string Keyword { get; set; }
    public int InputTokenCount { get; set; }
    public int OutputTokenCount { get; set; }
}

public class GenerateKeywordFromRequestCommandHandler : IRequestHandler<GenerateKeywordFromRequestCommand, GenerateKeywordFromRequestResponse>
{
    private readonly ICopilotHelper _copilotHelper;

    public GenerateKeywordFromRequestCommandHandler(
        ICopilotHelper copilotHelper)
    {
        _copilotHelper = copilotHelper;
    }
    public async Task<GenerateKeywordFromRequestResponse> Handle(GenerateKeywordFromRequestCommand request, CancellationToken cancellationToken)
    {
        var keyword = await _copilotHelper.GenerateKeyword(request.Request);

        return new GenerateKeywordFromRequestResponse
        {
            Keyword = keyword.Keyword,
            InputTokenCount = keyword.InputTokenCount,
            OutputTokenCount = keyword.OutputTokenCount
        };
    }
}