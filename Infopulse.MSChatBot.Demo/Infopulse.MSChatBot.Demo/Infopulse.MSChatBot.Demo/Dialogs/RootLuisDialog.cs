using System;
using System.Linq;
using System.Threading.Tasks;
using Infopulse.MSChatBot.Demo.Entities;
using Infopulse.MSChatBot.Demo.Helpers;
using Infopulse.MSChatBot.Interfaces.Entities;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Json;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json.Linq;
using ImpromptuInterface;

namespace Infopulse.MSChatBot.Demo.Dialogs
{
    [LuisModel("model id", "subscription key")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {                
        private IConfigForDialogs _configForDialogs;

        public RootLuisDialog(IConfigForDialogs configForDialogs)
        {
            _configForDialogs = configForDialogs;
        }

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task NoneDialog(IDialogContext context, LuisResult result)
        {
            string message = $"Sorry, I did not understand '{result.Query}'. Please try again.";
            await context.PostAsync(message);
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("TripRequest")]
        public async Task TripRequestDialog(IDialogContext context, LuisResult result)
        {
            context.Call(FormDialog.FromForm(BuildJsonFormForBuisnessTrip, FormOptions.PromptInStart), ResumeAfterTripRequestDialog);
        }

        [LuisIntent("ServiceDesk.Help")]
        [LuisIntent("OnDevice.Help")]
        [LuisIntent("Utilities.Help")]
        public async Task ServiceDeskHelpDialog(IDialogContext context, LuisResult result)
        {

            var sdq = new ServiceDeskQuery();

            if (result.Entities.Any())
                sdq.Entity = result.Entities.First().Entity;

            FormDialog<ServiceDeskQuery> orderForm = new FormDialog<ServiceDeskQuery>(sdq, ServiceDeskQuery.BuildForm, FormOptions.PromptInStart);
            context.Call(orderForm, this.ResumeAfterServiceDeskDialog);
        }

        [LuisIntent("Survey")]
        public async Task SurveyDialog(IDialogContext context, LuisResult result)
        {
            context.Call(new CategoryDialog(_configForDialogs), ResumeAfterSurveyDialog);
        }

        private async Task ResumeAfterSurveyDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Thanks for your answers. Goodbye!");
        }

        private async Task ResumeAfterTripRequestDialog(IDialogContext context, IAwaitable<JObject> result)
        {
            var answer = result.GetAwaiter().GetResult();
            TripRequest tr = new TripRequest()
            {
                Destination = answer["destination"].ToString(),
                Transport = answer["transport"].ToString(),
                Departure = answer["departure"].ToString(),
                Requestor = context.Activity.From.Name
            };

            var botConversationInfo = new
            {                
                UserId = context.Activity.From.Id,
                ChannelId = context.Activity.ChannelId,
                ConversationId = context.Activity.Conversation.Id,
                ServiceUrl = context.Activity.ServiceUrl,
                BotId = context.Activity.Recipient.Id
            }.ActLike<IBotConversationInfo>();

            int tripRequestId;
            _configForDialogs.TripManager.SaveTrip(_configForDialogs.TripRequestListConfig, tr, botConversationInfo, out tripRequestId);

            await context.PostAsync("Your trip request submitted with id " + tripRequestId + ", and we will notify your about status of request.");
        }

        private IForm<JObject> BuildJsonFormForBuisnessTrip()
        {
            var json = new FormDialogReader().Read("BuisnessTrip");
            var schema = JObject.Parse(json);
            return new FormBuilderJson(schema)
                .AddRemainingFields()
                .Build();
        }

        private async Task ResumeAfterServiceDeskDialog(IDialogContext context, IAwaitable<ServiceDeskQuery> result)
        {
            try
            {
                var r = await result;
                var reply = context.MakeMessage();
                reply.Text = $"**Your request is:**\n\n *Title:* {r.Title}\n\n *Entity:* {r.Entity}\n\n *Description:* {r.Description}";
                await context.PostAsync(reply);
            }
            catch (FormCanceledException e)
            {
                
            }
            catch (TooManyAttemptsException)
            {
               
            }
        }
    }
}