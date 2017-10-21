namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface ITripRequest
    {
        string Requestor { get; set; }
        string Departure { get; set; }
        string Destination { get; set; }
        string Transport { get; set; }
        int BotConversationInfo { get; set; }
    }
}
