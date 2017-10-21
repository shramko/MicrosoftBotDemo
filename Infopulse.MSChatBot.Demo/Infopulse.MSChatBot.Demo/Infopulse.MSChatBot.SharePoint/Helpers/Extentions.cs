using System.Collections.Generic;
using System.Linq;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.SharePoint.Entities;
using Microsoft.SharePoint.Client;

namespace Infopulse.MSChatBot.SharePoint.Helpers
{
    public static class Extentions
    {
        public static string ObjectToString(this object spObject)
        {
            if (spObject == null)
                return string.Empty;
            return spObject.ToString();
        }

        public static bool ObjectToBoolean(this object spObject)
        {
            if (spObject == null || spObject.ToString() == "null")
                return false;
            int i = 0;
            bool parsingSuccess = int.TryParse(spObject.ToString(), out i);
            if (parsingSuccess && i <= 0)
                return false;
            return true;
        }

        public static ICategory ToCategory(this ListItem item, IQuestionsCategoryListConfig categoryListConfig)
        {
            if (item == null)
                return new NullCategoryEntity();
            int questionId = 0;
            var questionIdParsing = item.TryParceLookupFieldToId(categoryListConfig.FirstQuestion, ref questionId);

            if (!questionIdParsing)
                return new NullCategoryEntity();

            ICategory result = new CategoryEntity()
            {
                QuestionId = questionId,
                Category = item[categoryListConfig.CategoryName].ObjectToString(),
                CategoryId = int.Parse(item[categoryListConfig.IdFieldName].ObjectToString())
            };
            return result;
        }

        public static List<ICategory> ToCategories(this List<ListItem> items, IQuestionsCategoryListConfig categoryListConfig)
        {
            return items.Select(x => x.ToCategory(categoryListConfig)).ToList();
        }

        public static IQuestion ToQuestion(this ListItem item, IQuestionsListConfig questionList)
        {
            if (item == null)
                return new NullQuestionEntity();

            IQuestion result = new QuestionEntity()
            {
                Question = item[questionList.QuestionFieldName].ObjectToString(),
                QuestionId = int.Parse(item[questionList.IdFieldName].ObjectToString())
            };
            return result;
        }

        public static IAnswer ToAnswer(this ListItem item, IAnswersConfigList answerList)
        {
            if (item == null)
                return new NullAnswerEntity();

            int questionId = 0;
            var questionIdParsing = item.TryParceLookupFieldToId(answerList.QuestionFieldame, ref questionId);

            if(!questionIdParsing)
                return new NullAnswerEntity();

            int nextQuestionId = 0;
            var nextQuestionIdParsing = item.TryParceLookupFieldToId(answerList.NextQuestionFieldame, ref nextQuestionId);

            IAnswer result = new AnswerEntity()
            {
                QuestionId = questionId,
                Answer = item[answerList.AnswerFieldName].ObjectToString(),
                AnswerId = int.Parse(item[answerList.IdFieldName].ObjectToString()),
                NextQuestionId = nextQuestionIdParsing? nextQuestionId:-1
            };
            return result;
        }
        

        public static List<IAnswer> ToAnswers(this List<ListItem> items, IAnswersConfigList answerList)
        {
            return items.Select(item => item.ToAnswer(answerList)).ToList();
        }

        public static bool TryParceLookupFieldToId(this ListItem spListItem, string fieldName, ref int lookupId)
        {
            FieldLookupValue lookup = spListItem?[fieldName] as FieldLookupValue;
            if (lookup == null) return false;
            lookupId = lookup.LookupId;
            return true;
        }

        public static IBotConversationInfo ToBotConversationInfo(this ListItem item, IBotConversationInfotListConfig botConversationInfotListConfig)
        {
            if (item == null)
                return null;

            BotConversationInfo result = new BotConversationInfo(item.Id)
            {
                BotId = item[botConversationInfotListConfig.BotId].ObjectToString(),
                ChannelId = item[botConversationInfotListConfig.ChannelId].ObjectToString(),
                ConversationId = item[botConversationInfotListConfig.ConversationId].ObjectToString(),
                UserId = item[botConversationInfotListConfig.UserId].ObjectToString(),
                ServiceUrl = item[botConversationInfotListConfig.ServiceUrl].ObjectToString()
            };
            return result;
        }

        public static List<IBotConversationInfo> ToBotConversationInfo(this List<ListItem> items,
            IBotConversationInfotListConfig botConversationInfotListConfig)
        {
            return items.Select(x => x.ToBotConversationInfo(botConversationInfotListConfig)).ToList();
        }
    }
}