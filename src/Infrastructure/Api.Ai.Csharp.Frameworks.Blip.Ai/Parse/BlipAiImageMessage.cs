using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Api.Ai.Domain.DataTransferObject.Response;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Lime.Messaging.Contents;
using Api.Ai.Domain.DataTransferObject.Response.Message;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Parse
{
    public class BlipAiImageMessageParse : IMessageParse<IList<Document>>
    {
        #region IBlipAiMessageParse Members

        public Task<IList<Document>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            DocumentCollection documentCollection = null;

            var imageMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as ImageMessageResponse;

            if (imageMessage != null)
            {
                documentCollection = new DocumentCollection
                {
                    ItemType = DocumentContainer.MediaType,
                    Items = new DocumentContainer[1]
                    {
                        new DocumentContainer
                        {
                            Value = new MediaLink
                            {
                                Type = MediaType.Parse(imageMessage.ImageUrl.ToMediaType()),
                                Uri = new Uri(imageMessage.ImageUrl)
                            }
                        }
                    },
                    Total = 1,
                };
            }

            return Task.FromResult<IList<Document>>(new List<Document>() { documentCollection });
        }

        #endregion

    }
}
