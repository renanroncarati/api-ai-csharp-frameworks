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
    public class BotFrameworkImageMessageParse : IMessageParse<IList<Activity>>
    {
        #region IMessageParse Members

        public Task<IList<Activity>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            var imageMessage = queryResponse.Result.Fulfillment.Messages[messageDescriptor.Index] as ImageMessageResponse;

            if (imageMessage != null)
            {
                var activity = new Activity();

                if (!string.IsNullOrEmpty(imageMessage.ImageUrl))
                {
                    activity.Attachments.Add(new Attachment()
                    {
                        ContentUrl = imageMessage.ImageUrl,
                        ContentType = imageMessage.ImageUrl.ToMediaType(),
                        ThumbnailUrl = imageMessage.ImageUrl
                    });
                }

                return Task.FromResult<IList<Activity>>(new List<Activity>() { activity });
            }

            return null;


        }

        #endregion

    }
}
