namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface ITripRequestListConfig : IBaseListConfig
    {
        string Requestor { get; }
        string Departure { get; }
        string Destination { get; }
        string Transport { get; }
        string BotConversationInfo { get; }
    }
}