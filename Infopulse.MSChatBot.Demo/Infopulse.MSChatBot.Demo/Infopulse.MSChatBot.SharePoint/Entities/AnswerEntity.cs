using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Entities
{
    [Serializable]
    public class AnswerEntity : IAnswer
    {
        public int AnswerId { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
        public int NextQuestionId { get; set; }

        public override string ToString() => Answer;
        
    }

    [Serializable]
    public class NullAnswerEntity : IAnswer
    {
        public int AnswerId { get { return -1; } set { } }
        public string Answer
        {
            get { return string.Empty; }
            set { }
        }
        public int QuestionId
        {
            get { return -1; }
            set { }
        }

        public int NextQuestionId
        {
            get { return -1; }
            set { }
        }

        public override string ToString() => Answer;
    }
}