using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using Infopulse.MSChatBot.Demo.Entities;
using Infopulse.MSChatBot.Demo.Helpers;
using Infopulse.MSChatBot.Interfaces.Entities;
using Infopulse.MSChatBot.Interfaces.Managers;
using Infopulse.MSChatBot.SharePoint.Helpers;
using Infopulse.MSChatBot.SharePoint.SharePoint;

namespace Infopulse.MSChatBot.Demo.Controllers
{
    //[BotAuthentication]
    public class ProactiveWebAPIController: ApiController
    {
        [HttpGet]
        [Route("api/ProactiveWebAPI")]
        public async Task<HttpResponseMessage> SendMessage(string message, int botConversationInfoId = 0)
        {            
            IBotLogger logger = new BotLogger();
            IEnumerable<string> headerValues;
            
            if (!Request.Headers.TryGetValues("key", out headerValues) 
                || headerValues.FirstOrDefault() != WebConfigurationManager.AppSettings["ProactiveMessageKey"].ToString())
            {
                logger.WriteMessage("Incorrect key value", "Info");
                var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                resp1.Content = new StringContent($"<html><body>Incorrect key value</body></html>", System.Text.Encoding.UTF8, @"text/html");
                return resp1;
            }

            IBotStorage storage = new SPStorage();
            IBotConversationInfotListConfig botConversationInfotListConfig = new BotConversationInfoListConfig();

            var conversations = new List<IBotConversationInfo>();

            if (botConversationInfoId > 0)            
                conversations.Add(storage.GetBotConversationInfoById(botConversationInfoId,botConversationInfotListConfig));            
            else            
                conversations = storage.GetAllUniqueBotConversationInfo(botConversationInfotListConfig);

            try
            {
                foreach (var con in conversations)
                {
                    try
                    {
                        var proMess = new ProactiveMessager(con);
                        if (!string.IsNullOrEmpty(proMess.FromId))
                        {
                            //var mes = string.Format("New category of questions '{0}' was added!", message);
                            await proMess.Resume(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.WriteExceprion(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteExceprion(ex);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent($"<html><body>Message sent, thanks.</body></html>",
                System.Text.Encoding.UTF8, @"text/html");
            return resp;
        }
    }
}