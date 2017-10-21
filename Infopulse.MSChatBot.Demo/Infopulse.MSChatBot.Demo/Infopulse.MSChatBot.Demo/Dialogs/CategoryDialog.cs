using System;
using System.Threading.Tasks;
using Infopulse.MSChatBot.Demo.Helpers;
using Infopulse.MSChatBot.Interfaces.Entities;
using Microsoft.Bot.Builder.Dialogs;

namespace Infopulse.MSChatBot.Demo.Dialogs
{
    [Serializable]
    public class CategoryDialog : IDialog<object>
    {
        IConfigForDialogs _configForDialogs;

        public CategoryDialog(IConfigForDialogs configForDialogs)
        {
            _configForDialogs = configForDialogs;
        }

        public async Task StartAsync(IDialogContext context)
        {
            ShowCategories(context);
        }

        private void ShowCategories(IDialogContext context)
        {
            var questionCategories = _configForDialogs.Storage.GetCategories(_configForDialogs.CategoryListConfig);
            PromptDialog.Choice(context, this.OnCategorySelected, questionCategories, "Please select category of questions: ", "Not a valid option");
        }

        private async Task OnCategorySelected(IDialogContext context, IAwaitable<ICategory> result)
        {
            try
            {
                var activity = await result;
                var questionId = activity.QuestionId;
                context.Call(new QuestionsDialog(_configForDialogs, questionId), ResumeAfterSurveyCompleted);
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                ShowCategories(context);
            }
        }

        private async Task ResumeAfterSurveyCompleted(IDialogContext context, IAwaitable<object> result)
        {
            var r = await result;
            context.Done(r);
        }
    }
}