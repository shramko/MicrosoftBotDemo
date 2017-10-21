using System;
using Infopulse.MSChatBot.Interfaces.Entities;

namespace Infopulse.MSChatBot.SharePoint.Entities
{
    public class BotConversationInfo : IBotConversationInfo
    {
        private readonly int _listItemId;
        public BotConversationInfo() { }

        public BotConversationInfo(int listId)
        {
            _listItemId = listId;
        }

        public int ListItemId => _listItemId;
        public string BotId { get; set; }
        public string UserId { get; set; }
        public string ConversationId { get; set; }
        public string ChannelId { get; set; }
        public string ServiceUrl { get; set; }

        public bool Equals(IBotConversationInfo obj)
        {
            if (Object.ReferenceEquals(obj, null)) return false;
            if (Object.ReferenceEquals(this, obj)) return true;
            return
                BotId.Equals(obj.BotId)
                && ServiceUrl.Equals(obj.ServiceUrl)
                && ChannelId.Equals(obj.ChannelId)
                && ConversationId.Equals(obj.ConversationId)
                && UserId.Equals(obj.UserId);
        }

        public override int GetHashCode()
        {
            var b = BotId.GetHashCode();
            var u = UserId.GetHashCode();
            var con = ConversationId.GetHashCode();
            var ch = ChannelId.GetHashCode();
            var s = ServiceUrl.GetHashCode();
            var r = b ^ u ^ con ^ ch ^ s;
            return r;
        }

    }
}
