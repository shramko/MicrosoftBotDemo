using System;
using System.Collections.Generic;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Entities
{
    [Serializable]
    public class QuestionEntity : IQuestion
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public List<IAnswer> Answers { get; set; }
        public override string ToString() => Question;
    }

    [Serializable]
    public class NullQuestionEntity : IQuestion
    {
        public int QuestionId
        {
            get { return -1; }
            set { }
        }
        public string Question
        {
            get { return string.Empty; }
            set { }
        }
        public List<IAnswer> Answers
        {
            get { return new List<IAnswer>(); }
            set { }
        }
        public override string ToString() => Question;
    }
}