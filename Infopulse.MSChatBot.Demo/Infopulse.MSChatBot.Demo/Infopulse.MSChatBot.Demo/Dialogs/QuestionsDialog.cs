using System;
using System.Threading.Tasks;
using ImpromptuInterface;
using Infopulse.MSChatBot.Demo.Helpers;
using Infopulse.MSChatBot.Interfaces.Entities;
using Microsoft.Bot.Builder.Dialogs;

namespace Infopulse.MSChatBot.Demo.Dialogs
{
    [Serializable]
    public class QuestionsDialog:IDialog<object>
    {
        private IConfigForDialogs _configForDialogs;
        private readonly int _firstQuestionId;

        public QuestionsDialog(IConfigForDialogs configForDialogs, int firstQuestionId)
        {
            _configForDialogs = configForDialogs;
            _firstQuestionId = firstQuestionId;
        }

        public async Task StartAsync(IDialogContext context)
        {
            ShowQuestion(context);
        }

        private void ShowQuestion(IDialogContext context)
        {
            var qa = _configForDialogs.Storage.GetQuestionWithAnswersById(_firstQuestionId, _configForDialogs.QuestionListConfig, _configForDialogs.AnswerConfig);
            if (qa.Answers.Count > 0)
                PromptDialog.Choice(context, this.OnAnswerSelected, qa.Answers, qa.Question, "Not a valid option");
        }

        private async Task OnAnswerSelected(IDialogContext context, IAwaitable<IAnswer> result)
        {
            try
            {
                var optionSelected = await result;

                var botConversationInfo = new
                {
                    UserId = context.Activity.From.Id,
                    ChannelId = context.Activity.ChannelId,
                    ConversationId = context.Activity.Conversation.Id,
                    ServiceUrl = context.Activity.ServiceUrl,
                    BotId = context.Activity.Recipient.Id
                }.ActLike<IBotConversationInfo>();

                var qr = new
                {
                    UserName = context.Activity.From.Name,
                    Answer = optionSelected,
                    BotConversationInfo = botConversationInfo
                }.ActLike<IQuestionResult>();
                
                _configForDialogs.Storage.SaveResult(qr, _configForDialogs.QuestionResultListConfig, botConversationInfo);

                var nextQuestionId = optionSelected.NextQuestionId;
                if (nextQuestionId <= 0)
                {
                    context.Done(optionSelected);
                    return;
                }

                var qa = _configForDialogs.Storage.GetQuestionWithAnswersById(nextQuestionId, _configForDialogs.QuestionListConfig, _configForDialogs.AnswerConfig);
                if(qa.Answers.Count >0)
                    PromptDialog.Choice(context, this.OnAnswerSelected, qa.Answers, qa.Question, "Not a valid option");
                else
                    context.Done(optionSelected);
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");
                ShowQuestion(context);
            }
        }
    }
}