using System;
using System.Collections.Generic;
using System.Linq;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.Interfaces.Managers;
using Infopulse.MSChatBot.Interfaces.Queries;
using Infopulse.MSChatBot.SharePoint.Entities;
using Infopulse.MSChatBot.SharePoint.Helpers;
using Microsoft.SharePoint.Client;

namespace Infopulse.MSChatBot.SharePoint.SharePoint
{
    [Serializable]
    public class SPStorage : IBotStorage, ITripManager
    {
        private IBotConversationInfotListConfig _botConversationInfotListConfig = new BotConversationInfoListConfig();
        private readonly IQueries _query = new CamlQueries();
        public List<ICategory> GetCategories(IQuestionsCategoryListConfig questionsCategoryListConfig)
        {
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = _query.GetAllCategories(questionsCategoryListConfig)
            };
            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(questionsCategoryListConfig.ListTitle);

            ListItemCollection listItems = spList.GetItems(camlQuery);
            SPContext.Instance.ClientCtx.Load(listItems);
            SPContext.Instance.ClientCtx.ExecuteQuery();
            return listItems.ToList().ToCategories(questionsCategoryListConfig);
        }

        public IQuestion GetQuestionById(int questionId, IQuestionsListConfig questionsListConfig)
        {
            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(questionsListConfig.ListTitle);
            ListItem listItem = spList.GetItemById(questionId);
            SPContext.Instance.ClientCtx.Load(listItem);
            SPContext.Instance.ClientCtx.ExecuteQuery();
            return listItem.ToQuestion(questionsListConfig);
        }

        public IQuestion GetQuestionWithAnswersById(int questionId, IQuestionsListConfig questionsListConfig,
            IAnswersConfigList answersConfigList)
        {
            if (questionId <= 0) return new NullQuestionEntity();
            var question = GetQuestionById(questionId, questionsListConfig);
            if (question.QuestionId <= 0) return new NullQuestionEntity();
            var answers = GetAnswersForQuestion(question.QuestionId, answersConfigList);
            question.Answers = answers;
            return question;
        }

        public List<IAnswer> GetAnswersForQuestion(int questionId, IAnswersConfigList answersConfigList)
        {
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = _query.GetAnswersForQuestionId(questionId, answersConfigList)
            };
            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(answersConfigList.ListTitle);

            ListItemCollection listItems = spList.GetItems(camlQuery);
            SPContext.Instance.ClientCtx.Load(listItems);
            SPContext.Instance.ClientCtx.ExecuteQuery();
            var result = listItems.ToList();
            return result.ToAnswers(answersConfigList);
        }

        public void SaveResult(IQuestionResult questionResult, IQuestionResultListConfig questionResultListConfig, IBotConversationInfo botConversationInfo)
        {
            var botInfoItemId = SaveBotConversationInfo(botConversationInfo);

            var questionLookup = new FieldLookupValue { LookupId = questionResult.Answer.QuestionId };
            var answerLookup = new FieldLookupValue { LookupId = questionResult.Answer.AnswerId };
            var botInfoLookup = new FieldLookupValue { LookupId = botInfoItemId };

            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(questionResultListConfig.ListTitle);
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem oListItem = spList.AddItem(itemCreateInfo);
            oListItem[questionResultListConfig.UserName] = questionResult.UserName;
            oListItem[questionResultListConfig.QuestionFieldName] = questionLookup;
            oListItem[questionResultListConfig.AnswerFieldName] = answerLookup;
            oListItem[questionResultListConfig.BotConversationInfo] = botInfoLookup;
            oListItem.Update();
            SPContext.Instance.ClientCtx.ExecuteQuery();
        }

        public List<IBotConversationInfo> GetAllUniqueBotConversationInfo(IBotConversationInfotListConfig botConversationInfotListConfig)
        {
            List<IBotConversationInfo> items = new List<IBotConversationInfo>();
            ListItemCollectionPosition position = null;
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = _query.GetAllBotConversationInfo(botConversationInfotListConfig, 1000)
            };
            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(botConversationInfotListConfig.ListTitle);

            while (true)
            {
                camlQuery.ListItemCollectionPosition = position;
                ListItemCollection listItems = spList.GetItems(camlQuery);
                SPContext.Instance.ClientCtx.Load(listItems);
                SPContext.Instance.ClientCtx.ExecuteQuery();
                position = listItems.ListItemCollectionPosition;
                var rl = listItems.ToList().ToBotConversationInfo(botConversationInfotListConfig);
                items.AddRange(rl);
                if (position == null) break;
            }
            var result = items.Distinct().ToList();
            return result;
        }

        public void SaveTrip(ITripRequestListConfig tripRequestListConfig,
            ITripRequest request, IBotConversationInfo botConversationInfo, out int requestId)
        {
            var botInfoItemId = SaveBotConversationInfo(botConversationInfo);
            //add trip request
            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(tripRequestListConfig.ListTitle);
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem oListItem = spList.AddItem(itemCreateInfo);
            oListItem[tripRequestListConfig.Requestor] = request.Requestor;
            oListItem[tripRequestListConfig.Departure] = request.Departure;
            oListItem[tripRequestListConfig.Destination] = request.Destination;
            oListItem[tripRequestListConfig.Transport] = request.Transport;
            oListItem[tripRequestListConfig.BotConversationInfo] = botInfoItemId;
            oListItem.Update();
            SPContext.Instance.ClientCtx.ExecuteQuery();
            requestId = oListItem.Id;
        }

        private int SaveBotConversationInfo(IBotConversationInfo botConversationInfo)
        {
            int botInfoItemId;
            List botInfoList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(_botConversationInfotListConfig.ListTitle);

            //check if bot info exist
            CamlQuery camlQuery = new CamlQuery
            {
                ViewXml = _query.IsBotConversationInfoExist(_botConversationInfotListConfig, botConversationInfo)
            };
            ListItemCollection listItems = botInfoList.GetItems(camlQuery);
            SPContext.Instance.ClientCtx.Load(listItems);
            SPContext.Instance.ClientCtx.ExecuteQuery();

            if (listItems.Any())
                botInfoItemId = listItems[0].Id;
            else
            {
                //add Bot Conversation Info
                ListItemCreationInformation botInfoItem = new ListItemCreationInformation();
                ListItem infoItem = botInfoList.AddItem(botInfoItem);
                infoItem[_botConversationInfotListConfig.BotId] = botConversationInfo.BotId;
                infoItem[_botConversationInfotListConfig.ChannelId] = botConversationInfo.ChannelId;
                infoItem[_botConversationInfotListConfig.ConversationId] = botConversationInfo.ConversationId;
                infoItem[_botConversationInfotListConfig.ServiceUrl] = botConversationInfo.ServiceUrl;
                infoItem[_botConversationInfotListConfig.UserId] = botConversationInfo.UserId;
                infoItem.Update();
                SPContext.Instance.ClientCtx.ExecuteQuery();
                botInfoItemId = infoItem.Id;
            }
            return botInfoItemId;
        }

        public IBotConversationInfo GetBotConversationInfoById(int botConversationInfoId, IBotConversationInfotListConfig botConversationInfotListConfig)
        {

            List spList = SPContext.Instance.ClientCtx.Web.Lists.GetByTitle(botConversationInfotListConfig.ListTitle);
            ListItem listItem = spList.GetItemById(botConversationInfoId);
            SPContext.Instance.ClientCtx.Load(listItem);
            SPContext.Instance.ClientCtx.ExecuteQuery();
            IBotConversationInfo bci = new BotConversationInfo(listItem.Id)
            {
                BotId = listItem[botConversationInfotListConfig.BotId].ObjectToString(),
                ChannelId = listItem[botConversationInfotListConfig.ChannelId].ObjectToString(),
                ConversationId = listItem[botConversationInfotListConfig.ConversationId].ObjectToString(),
                ServiceUrl = listItem[botConversationInfotListConfig.ServiceUrl].ObjectToString(),
                UserId = listItem[botConversationInfotListConfig.UserId].ObjectToString(),
            };
            return bci;
        }
    }
}
