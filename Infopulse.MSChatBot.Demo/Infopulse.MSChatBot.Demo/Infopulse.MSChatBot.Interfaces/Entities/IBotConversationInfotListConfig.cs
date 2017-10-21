namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IBotConversationInfotListConfig : IBaseListConfig
    {        
        string BotId { get; }
        string ConversationId { get; }
        string ChannelId { get; }
        string UserId { get; }
        string ServiceUrl { get; }
    }
}
