namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface ICategory
    {
        int CategoryId { get; set; }
        string Category { get; set; }
        int QuestionId { get; set; }
    }
}
