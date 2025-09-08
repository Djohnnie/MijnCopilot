using Microsoft.Extensions.Configuration;

namespace MijnCopilot.Application.Helpers.Agents;

public interface IPhotoCarouselAgentFactory : IAgentFactory { }

internal class PhotoCarouselAgentFactory : AgentFactoryBase, IPhotoCarouselAgentFactory
{
    private string _description = "An agent that has real-time knowledge on my displayed photos via PhotoCarousel (get information about current, previous and next displayed photo)";
    private string _instructions = @"
You should answer questions and execute commands regarding photos shown on my photo carousel:
- What is the current photo being shown
- What is the next photo being shown
- What was the previous photo being shown
Adhere to the following rules:
- Just use plain text, no markdown or any other formatting
- Separate every sentence with a [BR] as custom newline
- Only answer questions and execute commands that are related to my photo carousel
- If you don't know the answer, say you don't know or can't help with that
";

    protected override string AgentName => "PhotoCarouselAgent";
    protected override string AgentDescription => _description;
    protected override string AgentInstruction => _instructions;
    protected override bool HasPlugin => true;
    protected override string PluginName => "PhotoCarouselPlugin";
    protected override string McpEndpointConfig => "PHOTOCAROUSEL_MCP";
    protected override string McpName => "PhotoCarouselMcpClient";

    public PhotoCarouselAgentFactory(IConfiguration configuration) : base(configuration) { }
}