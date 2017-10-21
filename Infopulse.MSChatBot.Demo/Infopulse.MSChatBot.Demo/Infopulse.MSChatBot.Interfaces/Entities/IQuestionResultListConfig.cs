namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IQuestionResultListConfig:IBaseListConfig
    {
        string QuestionFieldName { get; }
        string AnswerFieldName { get; }
        string UserName { get; }
        string BotConversationInfo { get; }
    }
}
