using System.Collections.Generic;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.Interfaces.Managers
{
    public interface IBotStorage
    {
        List<ICategory> GetCategories(IQuestionsCategoryListConfig questionsCategoryListConfig);
        IQuestion GetQuestionById(int questionId, IQuestionsListConfig questionsListConfig);
        IQuestion GetQuestionWithAnswersById(int questionId, IQuestionsListConfig questionsListConfig, IAnswersConfigList answersConfigList);
        List<IAnswer> GetAnswersForQuestion(int questionId, IAnswersConfigList answersConfigList);
        void SaveResult(IQuestionResult questionResult, IQuestionResultListConfig questionResultListConfig, IBotConversationInfo botConversationInfo);
        List<IBotConversationInfo> GetAllUniqueBotConversationInfo(IBotConversationInfotListConfig botConversationInfotListConfig);
        IBotConversationInfo GetBotConversationInfoById(int botConversationInfoId, IBotConversationInfotListConfig botConversationInfotListConfig);
    }
}
