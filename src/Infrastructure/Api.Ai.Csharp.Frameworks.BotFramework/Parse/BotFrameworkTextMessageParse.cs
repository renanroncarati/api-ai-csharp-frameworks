using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Csharp.Frameworks.BotFramework.Extensions;
using Api.Ai.Domain.DataTransferObject.Response.Message;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Parse
{
    public class BotFrameworkTextMessageParse : IMessageParse<IList<Activity>>
    {
        #region IMessageParse Members

        public Task<IList<Activity>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            Activity activity = null;

            var textMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as TextMessageResponse;

            if (textMessage != null && !string.IsNullOrEmpty(textMessage.Speech))
            {
                activity = new Activity();

                if (!string.IsNullOrEmpty(textMessage.Speech))
                {
                    activity.Text = textMessage.Speech;
                }
            }

            return Task.FromResult<IList<Activity>>(new List<Activity>() { activity });
        }

        #endregion

    }
}
