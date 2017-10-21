using System;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.Interfaces.Queries;

namespace Infopulse.MSChatBot.SharePoint.SharePoint
{
	[Serializable]
	public class CamlQueries:IQueries
	{
		public string GetAllCategories(IQuestionsCategoryListConfig questionsCategoryListConfig)
		{
			return string.Format(
				@"<View Scope='RecursiveAll'>                          
					<Query> 
					   <OrderBy><FieldRef Name='{0}' /></OrderBy> 
					</Query> 
					<ViewFields>
						<FieldRef Name='{0}' />
						<FieldRef Name='{1}' />
						<FieldRef Name='{2}' />
					</ViewFields>        
				</View>", 
				questionsCategoryListConfig.CategoryName, 
				questionsCategoryListConfig.FirstQuestion,
				questionsCategoryListConfig.IdFieldName);
		}

		public string GetAnswersForQuestionId(int questionId, IAnswersConfigList answerList)
		{
			return string.Format(
				@"<View Scope='RecursiveAll'>  
						<Query> 
							<Where>
							  <Eq>
								 <FieldRef Name='{0}' LookupId='True' />
								 <Value Type='Lookup'>{1}</Value>
							  </Eq>
							</Where>
							<OrderBy>
								<FieldRef Name='ID' />
							</OrderBy> 
						</Query>                     
				</View>", answerList.QuestionFieldame, questionId);
		}

  //      public string GetSurveyResults(IQuestionResultListConfig questionResultListConfig)
		//{
		//	return string.Format(
		//		@"<View Scope='RecursiveAll'>  
		//				  <ViewFields>
		//				  <FieldRef Name='{0}' />
		//				  <FieldRef Name='{1}' />
		//				  <FieldRef Name='{2}' />
		//				  <FieldRef Name='{3}' />
		//				  <FieldRef Name='{4}' />
		//			   </ViewFields>
		//			   <RowLimit>300</RowLimit>
		//		</View>", 
		//		questionResultListConfig.UserId, 
		//		questionResultListConfig.BotId, 
		//		questionResultListConfig.ConversationId,
		//		questionResultListConfig.ChannelId,
		//		questionResultListConfig.ServiceUrl);
		//}

		public string IsBotConversationInfoExist(IBotConversationInfotListConfig botConversationInfotListConfig, IBotConversationInfo botConversationInfo)
		{
			return string.Format(
                @"<View Scope='RecursiveAll'>  					   
						<Query> 
							<Where>
								<And>
									<And>
										<And>
											<And>
												<Eq>
													<FieldRef Name='Title' />
													<Value Type='Text'>{0}</Value>
												</Eq>
												<Eq>
													<FieldRef Name='BotId' />
													<Value Type='Text'>{1}</Value>
												</Eq>
											</And>
											<Eq>
												<FieldRef Name='ConversationId' />
												<Value Type='Text'>{2}</Value>
											</Eq>
										</And>
										<Eq>
											<FieldRef Name='ChannelId' />
											<Value Type='Text'>{3}</Value>
										</Eq>
									</And>
									<Eq>
										<FieldRef Name='ServiceUrl' />
										<Value Type='Text'>{4}</Value>
									</Eq>
								</And>
							</Where> 
						</Query>
                        <ViewFields>
						    <FieldRef Name='ID' />
						    <FieldRef Name='{5}' />
						    <FieldRef Name='{6}' />
						    <FieldRef Name='{7}' />
						    <FieldRef Name='{8}' />
						    <FieldRef Name='{9}' />
					   </ViewFields>
					   <RowLimit>1</RowLimit>
				</View>",
                botConversationInfo.UserId,
                botConversationInfo.BotId,
                botConversationInfo.ConversationId,
                botConversationInfo.ChannelId,
                botConversationInfo.ServiceUrl,
                botConversationInfotListConfig.UserId,
				botConversationInfotListConfig.BotId,
				botConversationInfotListConfig.ConversationId,
				botConversationInfotListConfig.ChannelId,
				botConversationInfotListConfig.ServiceUrl
				);
		}

        public string GetAllBotConversationInfo(IBotConversationInfotListConfig botConversationInfotListConfig, int rowLimit)
        {
            return string.Format(
                @"<View Scope='RecursiveAll'>
                        <ViewFields>
						    <FieldRef Name='ID' />
						    <FieldRef Name='{0}' />
						    <FieldRef Name='{1}' />
						    <FieldRef Name='{2}' />
						    <FieldRef Name='{3}' />
						    <FieldRef Name='{4}' />
					   </ViewFields>
					   <RowLimit>{5}</RowLimit>
				</View>",
                botConversationInfotListConfig.UserId,
                botConversationInfotListConfig.BotId,
                botConversationInfotListConfig.ConversationId,
                botConversationInfotListConfig.ChannelId,
                botConversationInfotListConfig.ServiceUrl,
                rowLimit);
        }


    }
}