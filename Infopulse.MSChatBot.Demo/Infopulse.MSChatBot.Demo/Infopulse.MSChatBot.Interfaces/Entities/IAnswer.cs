namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IAnswer
    {
        int AnswerId { get; set; }
        string Answer { get; set; }
        int QuestionId { get; set; }
        int NextQuestionId { get; set; }
    }
}