using MijnCopilot.Agents.Base;
using MijnCopilot.Agents.Model;
using System.Text;

namespace MijnCopilot.Agents.Orchestration;

public interface IAgentOrchestrationManager
{
    Task<CopilotChatHistory> Chat(CopilotChatHistory chat);
}

internal class AgentOrchestrationManager : IAgentOrchestrationManager
{
    private readonly SummaryAgentFactory _summaryAgentFactory;
    private readonly OrchestratorAgentFactory _orchestratorAgentFactory;
    private readonly ReplyAgentFactory _replyAgentFactory;
    private readonly GeneralAgentFactory _generalAgentFactory;
    private readonly MijnThuisPowerAgentFactory _mijnThuisPowerAgentFactory;
    private readonly MijnThuisSolarAgentFactory _mijnThuisSolarAgentFactory;
    private readonly MijnThuisCarAgentFactory _mijnThuisCarAgentFactory;
    private readonly MijnThuisHeatingAgentFactory _mijnThuisHeatingAgentFactory;
    private readonly MijnThuisSmartLockAgentFactory _mijnThuisSmartLockAgentFactory;
    private readonly MijnSaunaAgentFactory _mijnSaunaAgentFactory;
    private readonly PhotoCarouselAgentFactory _photoCarouselAgentFactory;

    private readonly Dictionary<string, AgentFactoryBase> _agents;

    public AgentOrchestrationManager(
        SummaryAgentFactory summaryAgentFactory,
        OrchestratorAgentFactory orchestratorAgentFactory,
        ReplyAgentFactory replyAgentFactory,
        GeneralAgentFactory generalAgentFactory,
        MijnThuisPowerAgentFactory mijnThuisPowerAgentFactory,
        MijnThuisSolarAgentFactory mijnThuisSolarAgentFactory,
        MijnThuisCarAgentFactory mijnThuisCarAgentFactory,
        MijnThuisHeatingAgentFactory mijnThuisHeatingAgentFactory,
        MijnThuisSmartLockAgentFactory mijnThuisSmartLockAgentFactory,
        MijnSaunaAgentFactory mijnSaunaAgentFactory,
        PhotoCarouselAgentFactory photoCarouselAgentFactory)
    {
        _summaryAgentFactory = summaryAgentFactory;
        _orchestratorAgentFactory = orchestratorAgentFactory;
        _replyAgentFactory = replyAgentFactory;
        _generalAgentFactory = generalAgentFactory;
        _mijnThuisPowerAgentFactory = mijnThuisPowerAgentFactory;
        _mijnThuisSolarAgentFactory = mijnThuisSolarAgentFactory;
        _mijnThuisCarAgentFactory = mijnThuisCarAgentFactory;
        _mijnThuisHeatingAgentFactory = mijnThuisHeatingAgentFactory;
        _mijnThuisSmartLockAgentFactory = mijnThuisSmartLockAgentFactory;
        _mijnSaunaAgentFactory = mijnSaunaAgentFactory;
        _photoCarouselAgentFactory = photoCarouselAgentFactory;

        _agents = new Dictionary<string, AgentFactoryBase>
        {
            { "General", _generalAgentFactory },
            { "MijnThuisPower", _mijnThuisPowerAgentFactory },
            { "MijnThuisSolar", _mijnThuisSolarAgentFactory },
            { "MijnThuisCar", _mijnThuisCarAgentFactory },
            { "MijnThuisHeating", _mijnThuisHeatingAgentFactory },
            { "MijnThuisSmartLock", _mijnThuisSmartLockAgentFactory },
            { "MijnSauna", _mijnSaunaAgentFactory },
            { "PhotoCarousel", _photoCarouselAgentFactory }
        };
    }

    public async Task<CopilotChatHistory> Chat(CopilotChatHistory chat)
    {
        var workingChat = chat.Copy();

        // 1. Summarize the conversation so far to focus on the question asked by the user.
        var summaryAgent = await _summaryAgentFactory.Create();
        var summaryResponse = await summaryAgent.Chat(workingChat);
        workingChat.InputTokenCount += summaryResponse.InputTokenCount;
        workingChat.OutputTokenCount += summaryResponse.OutputTokenCount;
        workingChat.AddDebug(isQuestion: true, summaryResponse.Response, summaryAgent.Agent.Name);

        // 2. 
        var agents = string.Join(",", _agents.Select(x => $"Name: {x.Key}; Description: {x.Value.AgentDescription}"));
        var systemPromptBuilder = new StringBuilder();
        systemPromptBuilder.AppendLine("Based on the conversation so far, which agents are best suited to respond to the questions asked?");
        systemPromptBuilder.AppendLine($"The available agents are: {agents}.");
        systemPromptBuilder.AppendLine("If the question exists out of multiple sub-questions, start by rewriting the original question in multiple distinct questions.");
        systemPromptBuilder.AppendLine("Next: For each question, select the most appropriate agent from the list of available agents.");
        systemPromptBuilder.AppendLine("Respond with a line for each question, including the rewritten question followed by a semicolon, followed by the name of the agent that should answer that question.");
        workingChat.AddSystemMessage(systemPromptBuilder.ToString());

        // 3. Orchestrate the conversation and 
        var orchestratorAgent = await _orchestratorAgentFactory.Create();
        var orchestratorResponse = await orchestratorAgent.Chat(workingChat);
        workingChat.InputTokenCount += orchestratorResponse.InputTokenCount;
        workingChat.OutputTokenCount += orchestratorResponse.OutputTokenCount;

        var agentTasks = new List<(string Question, Task<CopilotAgentResponse> Task)>();
        foreach (var line in orchestratorResponse.Response.Split("\n"))
        {
            var parts = line.Split(";");
            var agentFactory = _agents[parts[1].Trim()];
            var agent = await agentFactory.Create();
            agentTasks.Add((parts[0], agent.Chat(new CopilotChatHistory(parts[0], CopilotChatRole.User))));
        }

        var responses = await Task.WhenAll(agentTasks.Select(x => x.Task));

        var replyAgent = await _replyAgentFactory.Create();
        var replyAgentChat = new CopilotChatHistory();
        replyAgentChat.AddUserMessage(summaryResponse.Response);
        foreach (var (agentTask, response) in agentTasks.Zip(responses))
        {
            replyAgentChat.AddAssistantMessage(response.Response);
            workingChat.InputTokenCount += response.InputTokenCount;
            workingChat.OutputTokenCount += response.OutputTokenCount;
            workingChat.AddDebug(isQuestion: true, agentTask.Question, response.AgentName);
            workingChat.AddDebug(isQuestion: false, response.Response, response.AgentName);
        }

        var replyResponse = await replyAgent.Chat(replyAgentChat);
        workingChat.AddDebug(isQuestion: false, replyResponse.Response, replyAgent.Agent.Name);

        workingChat.AddAssistantMessage(replyResponse.Response);
        workingChat.LastAssistantMessage = replyResponse.Response;
        workingChat.AgentName = string.Join(", ", responses.Select(x => x.AgentName).Distinct());
        workingChat.InputTokenCount += replyResponse.InputTokenCount;
        workingChat.OutputTokenCount += replyResponse.OutputTokenCount;
        return workingChat;
    }
}