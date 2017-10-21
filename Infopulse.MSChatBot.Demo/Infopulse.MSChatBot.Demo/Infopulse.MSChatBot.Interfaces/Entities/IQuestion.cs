using System.Collections.Generic;

namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IQuestion
    {
        int QuestionId { get; set; }
        string Question { get; set; }
        List<IAnswer> Answers { get; set; }
    }
}