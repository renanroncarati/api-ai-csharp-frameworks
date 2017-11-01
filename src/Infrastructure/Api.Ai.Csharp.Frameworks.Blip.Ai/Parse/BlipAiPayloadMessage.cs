using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Domain.DataTransferObject.Response.Message;
using Lime.Messaging.Contents;
using Lime.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Parse
{
    public class BlipAiPayloadMessageParse : IMessageParse<IList<Document>>
    {
        #region IBlipAiMessageParse Members

        public Task<IList<Document>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            List<Document> documents = null;

            var payloadMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as PayloadMessageResponse;

            if (payloadMessage != null)
            {
                documents = new List<Document>();

                var payload = JsonConvert.DeserializeObject<PlatformPayload>(payloadMessage.Payload.ToString());

                if (payload != null && payload.Facebook != null && payload.Facebook.Attachment != null
                && payload.Facebook.Attachment.Payload != null && !string.IsNullOrEmpty(payload.Facebook.Attachment.Payload.Url))
                {
                    documents.Add(new MediaLink
                    {
                        Type = MediaType.Parse(payload.Facebook.Attachment.Payload.Url.ToMediaType()),
                        Uri = new Uri(payload.Facebook.Attachment.Payload.Url)
                    });
                }

            }

            return Task.FromResult<IList<Document>>(documents);
        }

        #endregion

    }
}
