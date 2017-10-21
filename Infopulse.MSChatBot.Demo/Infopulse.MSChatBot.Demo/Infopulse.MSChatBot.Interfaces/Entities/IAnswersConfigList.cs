namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IAnswersConfigList:IBaseListConfig
    {
        string AnswerFieldName { get; }
        string QuestionFieldame { get; }
        string NextQuestionFieldame { get; }
    }
}