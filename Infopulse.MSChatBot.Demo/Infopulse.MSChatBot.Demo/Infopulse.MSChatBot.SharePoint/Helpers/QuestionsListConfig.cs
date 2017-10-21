using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class QuestionsListConfig:IQuestionsListConfig
    {
        public string ListTitle => "Questions";
        public string IdFieldName => "ID";
        public string QuestionFieldName => "Question";
        public string IsFirstQuestionFieldName => "IsFirstQuestion";
    }
}