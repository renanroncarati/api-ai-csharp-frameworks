using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using Api.Ai.Csharp.Frameworks.BotFramework.Interfaces;
using Api.Ai.ApplicationService.Factories;
using Api.Ai.Domain.DataTransferObject.Request;

namespace Api.Ai.Csharp.Frameworks.Example.Bot.Application
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        #region Private fields

        private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
        private readonly IBotFrameworkMessageTranslator _botFrameworkMessageTranslator;

        #endregion

        #region Constructor

        public MessagesController(IBotFrameworkMessageTranslator botFrameworkMessageTranslator, IApiAiAppServiceFactory apiAiAppServiceFactory)
        {
            _apiAiAppServiceFactory = apiAiAppServiceFactory;
            _botFrameworkMessageTranslator = botFrameworkMessageTranslator;
        }

        #endregion

        #region Private Methods

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

        #endregion
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));

                var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", "YOUR_ACCESS_TOKEN");

                var queryRequest = new QueryRequest
                {
                    SessionId = "1",
                    Query = new string[] { activity.Text },
                    Lang = Api.Ai.Domain.Enum.Language.English
                };

                var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

                var activities = await _botFrameworkMessageTranslator.TranslateAsync(queryResponse, activity);

                foreach (var reply in activities)
                {
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

    }
}