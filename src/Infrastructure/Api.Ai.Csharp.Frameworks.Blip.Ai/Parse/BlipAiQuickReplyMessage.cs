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

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Parse
{
    public class BlipAiQuickReplyMessageParse : IMessageParse<IList<Document>>
    {
        #region IBlipAiMessageParse Members

        public Task<IList<Document>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            var quickReplayMessageCollection = queryResponse.ToQuickReplaies();

            if (quickReplayMessageCollection != null)
            {
                var documents = new List<Document>();

                for (int i = 0; i < quickReplayMessageCollection.Count; i++)
                {
                    var document = new Select
                    {
                        Scope = SelectScope.Immediate,
                        Options = new SelectOption[quickReplayMessageCollection[i].Replies.Count()],
                        Text = quickReplayMessageCollection[i].Title
                    };

                    if (quickReplayMessageCollection[i].Replies != null)
                    {
                        for (int j = 0; j < quickReplayMessageCollection[i].Replies.Count(); j++)
                        {
                            document.Options[j] = new SelectOption
                            {
                                Text = quickReplayMessageCollection[i].Replies[j],
                                Order = j,
                                Value = new PlainText { Text = quickReplayMessageCollection[i].Replies[j] }
                            };
                        }
                    }

                    documents.Add(document);
                }

                return Task.FromResult<IList<Document>>(documents);
            }

            return null;
        }

        #endregion

    }
}
