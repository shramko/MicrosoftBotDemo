using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Infopulse.MSChatBot.Demo.Dialogs;
using Infopulse.MSChatBot.Demo.Helpers;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.SharePoint.Helpers;
using Infopulse.MSChatBot.SharePoint.SharePoint;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Infopulse.MSChatBot.Demo
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                if (!string.IsNullOrEmpty(activity.Text))
                {
                    string msg = activity.Text.ToLower().Trim();
                    if (msg == "start over" || msg == "exit" || msg == "quit" || msg == "done" ||
                        msg == "start again" || msg == "restart" || msg == "leave" || msg == "reset")
                    {
                        //This is where the conversation gets reset!
                        ConnectorClient client = new ConnectorClient(new Uri(activity.ServiceUrl));
                        var reply = activity.CreateReply();
                        reply.Text = "Goodbye!";
                        client.Conversations.ReplyToActivity(reply);
                        activity.GetStateClient().BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                SPStorage storage = new SPStorage();
                IQuestionsCategoryListConfig categoryListConfig = new QuestionsCategoryListConfig();
                IQuestionsListConfig questionsListConfig = new QuestionsListConfig();
                IAnswersConfigList answersConfigList = new AnswersListConfig();
                IQuestionResultListConfig questionResultListConfig = new QuestionResultListConfig();
                ITripRequestListConfig tripRequestListConfig = new TripRequestListConfig();
                IConfigForDialogs configForDialogs = new ConfigForDialogs(storage, storage, categoryListConfig, questionsListConfig, answersConfigList, questionResultListConfig, tripRequestListConfig);

                await Conversation.SendAsync(activity, () => new RootLuisDialog(configForDialogs));
                //await Conversation.SendAsync(activity, () => new RootDialog(configForDialogs));

            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}