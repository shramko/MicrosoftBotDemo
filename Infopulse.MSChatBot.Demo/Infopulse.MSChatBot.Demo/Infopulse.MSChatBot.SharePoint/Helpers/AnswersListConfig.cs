using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class AnswersListConfig:IAnswersConfigList
    {
        public string ListTitle => "Answers";
        public string IdFieldName => "ID";
        public string AnswerFieldName => "Answer";
        public string QuestionFieldame => "Question";
        public string NextQuestionFieldame => "NextQuestion";
    }
}