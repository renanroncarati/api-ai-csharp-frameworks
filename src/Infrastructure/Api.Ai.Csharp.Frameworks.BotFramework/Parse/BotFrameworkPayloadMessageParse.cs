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
using Newtonsoft.Json;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Parse
{
    public class BotFrameworkPayloadMessageParse : IMessageParse<IList<Activity>>
    {
        #region IMessageParse Members

        public Task<IList<Activity>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            Activity activity = null;

            var payloadMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as PayloadMessageResponse;

            if (payloadMessage != null)
            {
                activity = new Activity();
                
                    var payload = JsonConvert.DeserializeObject<PlatformPayload>(payloadMessage.Payload.ToString());

                    if (payload != null && payload.Facebook != null && payload.Facebook.Attachment != null
                        && payload.Facebook.Attachment.Payload != null && !string.IsNullOrEmpty(payload.Facebook.Attachment.Payload.Url))
                    {
                        activity.Attachments.Add(new Attachment
                        {
                            ContentUrl = payload.Facebook.Attachment.Payload.Url,
                            ContentType = payload.Facebook.Attachment.Payload.Url.ToMediaType()
                        });
                    }
                
            }
            
            return Task.FromResult<IList<Activity>>(new List<Activity>() { activity });
        }

        #endregion

    }
}
