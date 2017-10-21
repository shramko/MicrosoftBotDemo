namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IQuestionsListConfig:IBaseListConfig
    {
        string QuestionFieldName { get; }
        string IsFirstQuestionFieldName { get; }
    }
}