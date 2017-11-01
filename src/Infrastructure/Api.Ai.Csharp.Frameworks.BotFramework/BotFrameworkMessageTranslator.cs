using Api.Ai.Csharp.Frameworks.BotFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Domain.DataTransferObject.Response;
using Microsoft.Bot.Connector;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Csharp.Frameworks.BotFramework.Extensions;
using Newtonsoft.Json;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Factories;

namespace Api.Ai.Csharp.Frameworks.BotFramework
{
    public class BotFrameworkMessageTranslator : IBotFrameworkMessageTranslator
    {
        #region Private Fields
        private readonly IMessageParseFactory _messageParseFactory;
        #endregion

        #region Constructor

        public BotFrameworkMessageTranslator(IMessageParseFactory messageParseFactory)
        {
            _messageParseFactory = messageParseFactory;
        }

        #endregion

        #region Private Methods

        private void CreateReply(Activity activity, IList<Activity> activityParsedList)
        {            
            foreach (var activityParsed in activityParsedList)
            {
                activityParsed.From = activity.From;
                activityParsed.Recipient = activity.Recipient;
                activityParsed.Conversation = new ConversationAccount
                {
                    Id = activity.Conversation.Id
                };
            }
        }

        #endregion

        #region IBotFrameworkMessageTranslator Members

        public async Task<IList<Activity>> TranslateAsync(QueryResponse queryResponse, Activity activity)
        {
            var result = new List<Activity>();
            
            var messageDescriptors = queryResponse.ToMessageDescriptors();

            foreach (var messageDescriptor in messageDescriptors)
            {
                var messageParse = _messageParseFactory.Create<IList<Activity>>(messageDescriptor.Type);
                var activities = await messageParse.ParseAsync(queryResponse, messageDescriptor);

                CreateReply(activity, activities);

                result.AddRange(activities);
            }

            return result;
        }

        #endregion
    }
}
