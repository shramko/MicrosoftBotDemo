using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class QuestionsCategoryListConfig:IQuestionsCategoryListConfig
    {
        public string ListTitle => "Categories of questions";
        public string IdFieldName => "ID";
        public string CategoryName => "Title";
        public string FirstQuestion => "FirstQuestion";
    }
}
