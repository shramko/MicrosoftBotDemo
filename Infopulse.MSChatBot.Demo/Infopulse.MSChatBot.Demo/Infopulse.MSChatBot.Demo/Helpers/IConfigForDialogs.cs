using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.Interfaces.Managers;

namespace Infopulse.MSChatBot.Demo.Helpers
{
    public interface IConfigForDialogs
    {
        IQuestionsListConfig QuestionListConfig { get; }
        IQuestionsCategoryListConfig CategoryListConfig { get; }
        IAnswersConfigList AnswerConfig { get; }
        IQuestionResultListConfig QuestionResultListConfig { get; }
        ITripRequestListConfig TripRequestListConfig { get; }
        IBotStorage Storage { get; }
        ITripManager TripManager { get; }
    }
}