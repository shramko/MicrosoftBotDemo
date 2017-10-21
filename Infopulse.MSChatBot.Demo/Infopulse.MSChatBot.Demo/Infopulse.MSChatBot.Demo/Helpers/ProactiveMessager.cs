using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using Infopulse.MSChatBot.Interfaces.Entities;
using Microsoft.Bot.Connector;

namespace Infopulse.MSChatBot.Demo.Helpers
{
    public class ProactiveMessager
    {
        public readonly string FromId;
        public readonly string UserId;
        public readonly string ServiceUrl;
        public readonly string ChannelId;
        public string ConversationId;

        public ProactiveMessager(IBotConversationInfo questionResultBotData)
        {
            ChannelId = questionResultBotData.ChannelId;
            ConversationId = questionResultBotData.ConversationId;
            FromId = questionResultBotData.BotId;
            ServiceUrl = questionResultBotData.ServiceUrl;
            UserId = questionResultBotData.UserId;
        }

        public ProactiveMessager(string fromId, string userId, string serviceUrl, string channelId, string conversationId)
        {
            FromId = fromId;
            UserId = userId;
            ServiceUrl = serviceUrl;
            ChannelId = channelId;
            ConversationId = conversationId;
        }

        public async Task Resume(string mes)
        {
            MicrosoftAppCredentials.TrustServiceUrl(ServiceUrl);
            var userAccount = new ChannelAccount(UserId);
            var botAccount = new ChannelAccount(FromId);
            var microsoftAppId = WebConfigurationManager.AppSettings["MicrosoftAppId"];
            var microsoftAppPassword = WebConfigurationManager.AppSettings["MicrosoftAppPassword"];
            var connector = new ConnectorClient(new Uri(ServiceUrl), microsoftAppId, microsoftAppPassword);

            IMessageActivity message = Activity.CreateMessageActivity();
            if (!string.IsNullOrEmpty(ConversationId) && !string.IsNullOrEmpty(ChannelId))
            {
                message.ChannelId = ChannelId;
            }
            else
            {
                ConversationId = (await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount)).Id;
            }
            message.From = botAccount;
            message.Recipient = userAccount;
            message.Conversation = new ConversationAccount(id: ConversationId);
            message.Text = mes;
            message.Locale = "en-Us";
            await connector.Conversations.SendToConversationAsync((Activity)message);
        }
    }
}