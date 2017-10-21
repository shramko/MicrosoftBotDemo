using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class QuestionResultListConfig:IQuestionResultListConfig
    {
        public string ListTitle => "Results of survey";
        public string IdFieldName => "ID";
        public string QuestionFieldName => "Question";
        public string AnswerFieldName => "Answer";
        public string UserName => "Title";
        public string BotConversationInfo => "BotConversationInfo";
    }
}
