using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Lime.Protocol;
using Lime.Messaging.Contents;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Domain.DataTransferObject.Response.Message;
using Newtonsoft.Json;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Factories;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai
{
    public class BlipAiMessageTranslator : IBlipAiMessageTranslator
    {
        #region Private Fields
        private readonly IMessageParseFactory _messageParseFactory;
        #endregion

        #region Constructor

        public BlipAiMessageTranslator(IMessageParseFactory messageParseFactory)
        {
            _messageParseFactory = messageParseFactory;
        }

        #endregion

        #region Private Methods
        
        private List<Document> GetTextMessages(QueryResponse queryResponse)
        {
            var documents = new List<Document>();

            var textMessageCollection = queryResponse.ToTexts();

            if (textMessageCollection != null)
            {
                foreach (var textMessage in textMessageCollection)
                {
                    if (!string.IsNullOrEmpty(textMessage.Speech))
                    {
                        documents.Add(new PlainText
                        {
                            Text = textMessage.Speech
                        });
                    }
                }
            }

            return documents;
        }

        #endregion

        #region IMessageTranslator members

        public async Task<IList<Document>> TranslateAsync(QueryResponse queryResponse)
        {
            var result = new List<Document>();

            var messageDescriptors = queryResponse.ToMessageDescriptors();

            foreach (var messageDescriptor in messageDescriptors)
            {
                var messageParse = _messageParseFactory.Create<IList<Document>>(messageDescriptor.Type);
                var documents = await messageParse.ParseAsync(queryResponse, messageDescriptor);
                result.AddRange(documents);
            }

            return result;
        }

        #endregion

    }
}
