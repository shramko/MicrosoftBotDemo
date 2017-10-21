using System;
using System.Threading.Tasks;
using Infopulse.MSChatBot.Demo.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Infopulse.MSChatBot.Demo.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private IConfigForDialogs _configForDialogs;

        public RootDialog(IConfigForDialogs configForDialogs)
        {
            _configForDialogs = configForDialogs;
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as IMessageActivity;
            context.Call(new CategoryDialog(_configForDialogs), ResumeAfterNewOrderDialog);
        }

        private async Task ResumeAfterNewOrderDialog(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Goodbye!");

            // Again, wait for the next message from the user.
            context.Wait(this.MessageReceivedAsync);
        }

    }
}