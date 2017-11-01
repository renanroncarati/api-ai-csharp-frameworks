using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Domain.DataTransferObject.Response.Message;
using Lime.Messaging.Contents;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Parse
{
    public class BlipAiTextMessageParse : IMessageParse<IList<Document>>
    {
        #region IBlipAiMessageParse Members

        public Task<IList<Document>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            var documents = new List<Document>();

            var textMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as TextMessageResponse;

            if (textMessage != null)
            {
                if (!string.IsNullOrEmpty(textMessage.Speech))
                {
                    documents.Add(new PlainText
                    {
                        Text = textMessage.Speech
                    });
                }
            }

            return Task.FromResult<IList<Document>>(documents);
        }

        #endregion

    }
}
