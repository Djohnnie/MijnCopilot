using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers;

public interface IPhotoCarouselAgentFactory : IAgentFactory
{
    IAgent Create();
}

internal class PhotoCarouselAgentFactory : AgentFactoryBase, IPhotoCarouselAgentFactory
{
    protected override string AgentName => "PhotoCarouselAgent";
    protected override string AgentDescription => "";
    protected override string AgentInstruction => "";

    public PhotoCarouselAgentFactory(IConfiguration configuration) : base(configuration) { }
}