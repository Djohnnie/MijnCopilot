using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IPhotoCarouselAgentFactory : IAgentFactory { }

internal class PhotoCarouselAgentFactory : AgentFactoryBase, IPhotoCarouselAgentFactory
{
    private string _instructions = "You should answer questions regarding my photos (what is shown now on the PhotoCarousel) to the best of your ability";

    protected override string AgentName => "PhotoCarouselAgent";
    protected override string AgentDescription => _instructions;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "PhotoCarouselPlugin";
    protected override string McpEndpointConfig => "PHOTOCAROUSEL_MCP";
    protected override string McpName => "PhotoCarouselMcpClient";

    public PhotoCarouselAgentFactory(IConfiguration configuration) : base(configuration) { }
}