using System;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.Interfaces.Managers;

namespace Infopulse.MSChatBot.Demo.Helpers
{

    [Serializable]
    public class ConfigForDialogs : IConfigForDialogs
    {
        public ConfigForDialogs(IBotStorage storage, ITripManager tripManager,
            IQuestionsCategoryListConfig categoryListConfig, 
            IQuestionsListConfig questionsListConfig,
            IAnswersConfigList answersConfigList,
            IQuestionResultListConfig questionResultListConfig,
            ITripRequestListConfig tripRequestListConfig)
        {
            Storage = storage;
            TripManager = tripManager;
            CategoryListConfig = categoryListConfig;
            AnswerConfig = answersConfigList;
            QuestionListConfig = questionsListConfig;
            QuestionResultListConfig = questionResultListConfig;
            TripRequestListConfig = tripRequestListConfig;
        }

        public IQuestionsListConfig QuestionListConfig { get; }
        public IQuestionsCategoryListConfig CategoryListConfig { get; }
        public IAnswersConfigList AnswerConfig { get; }
        public IQuestionResultListConfig QuestionResultListConfig { get; }
        public ITripRequestListConfig TripRequestListConfig { get; }
        public IBotStorage Storage { get; }
        public ITripManager TripManager { get; }
    }
}