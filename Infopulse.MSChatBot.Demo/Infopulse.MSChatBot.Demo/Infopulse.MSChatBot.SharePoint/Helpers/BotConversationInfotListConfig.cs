using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    [Serializable]
    public class BotConversationInfoListConfig : IBotConversationInfotListConfig
    {
        public string ListTitle => "BotConversationInfo";
        public string IdFieldName => "ID";
        public string BotId => "BotId";
        public string ConversationId => "ConversationId";
        public string ChannelId => "ChannelId";
        public string UserId => "Title";
        public string ServiceUrl => "ServiceUrl";
    }
}
