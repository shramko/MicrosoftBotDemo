namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IQuestionResult
    {
        string UserName { get; set; }
        IAnswer Answer { get; set; }
        IBotConversationInfo BotConversationInfo { get; set; }
    }
}
