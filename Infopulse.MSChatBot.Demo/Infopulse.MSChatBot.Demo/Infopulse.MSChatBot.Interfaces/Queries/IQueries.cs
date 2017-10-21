using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.Interfaces.Queries
{
    public interface IQueries
    {
        string GetAllCategories(IQuestionsCategoryListConfig questionsCategoryListConfig);
        string GetAnswersForQuestionId(int questionId, IAnswersConfigList answerList);
        //string GetSurveyResults(IQuestionResultListConfig questionResultListConfig);
        string IsBotConversationInfoExist(IBotConversationInfotListConfig botConversationInfotListConfig, IBotConversationInfo botConversationInfo);
        string GetAllBotConversationInfo(IBotConversationInfotListConfig botConversationInfotListConfig, int rowLimit);
    }
}
