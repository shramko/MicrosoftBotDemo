using System;

namespace Infopulse.MSChatBot.Interfaces.Entities
{
    public interface IBotConversationInfo: IEquatable<IBotConversationInfo>
    {
        int ListItemId { get; }
        string BotId { get; set; }
        string UserId { get; set; }
        string ConversationId { get; set; }
        string ChannelId { get; set; }
        string ServiceUrl { get; set; }
    }
}
