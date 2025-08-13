using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using MijnCopilot.Application.DependencyInjection;

namespace MijnCopilot.Application.Helpers;

public interface ICopilotHelper
{
    Task<string> GenerateKeyword(string request);
    //Task Chat(string prompt);
    //Task Reduce(string prompt);
}

public class CopilotHelper : ICopilotHelper
{
    private readonly IMediator _mediator;
    private readonly IKeywordAgent _keywordAgent;
    private readonly Kernel _kernel;

    public CopilotHelper(
        IMediator mediator,
        IKeywordAgent keywordAgent,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _keywordAgent = keywordAgent;
        _kernel = InitializeCopilot(configuration);
    }

    public Kernel InitializeCopilot(IConfiguration configuration)
    {
        var deployment = configuration["AZUREOPENAI_DEPLOYMENT"];
        var endpoint = configuration["AZUREOPENAI_ENDPOINT"];
        var key = configuration["AZUREOPENAI_KEY"];

        var builder = Kernel.CreateBuilder();
        builder.AddAzureOpenAIChatCompletion(deployment, endpoint, key);
        builder.Services.AddSingleton(configuration);
        builder.Services.AddApplicationServices(configuration);
        //builder.Plugins.AddFromType<CopilotFunctions>();

        return builder.Build();
    }

    public Task<string> GenerateKeyword(string request)
    {
        return _keywordAgent.GenerateKeyword(_kernel, request);
    }
}