using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Domain.DataTransferObject.Response.Message;
using Lime.Messaging.Contents;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Parse
{
    public class BlipAiCardMessageParse : IMessageParse<IList<Document>>
    {
        #region Private Methods

        private DocumentContainer GetHeader(CardMessageResponse cardMessageResponse)
        {
            return new DocumentContainer
            {
                Value = new MediaLink
                {
                    Title = !string.IsNullOrEmpty(cardMessageResponse.Title) ? cardMessageResponse.Title : null,
                    Text = !string.IsNullOrEmpty(cardMessageResponse.Subtitle) ? cardMessageResponse.Subtitle : null,
                    Uri = !string.IsNullOrEmpty(cardMessageResponse.ImageUrl) ? new Uri(cardMessageResponse.ImageUrl) : default(Uri),
                    Type = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.Bitmap)
                }
            };
        }

        private DocumentSelectOption[] GetOptions(CardMessageResponse cardMessageResponse)
        {
            DocumentSelectOption[] options = null;

            if (cardMessageResponse.Buttons != null && cardMessageResponse.Buttons.Count() > 0)
            {
                options = new DocumentSelectOption[cardMessageResponse.Buttons.Count()];

                for (int j = 0; j < cardMessageResponse.Buttons.Count(); j++)
                {
                    if (!string.IsNullOrEmpty(cardMessageResponse.Buttons[j].Postback))
                    {
                        if (cardMessageResponse.Buttons[j].Postback.Contains("http"))
                        {
                            options[j] = new DocumentSelectOption
                            {
                                Label = new DocumentContainer
                                {
                                    Value = new WebLink
                                    {
                                        Title = !string.IsNullOrEmpty(cardMessageResponse.Buttons[j].Text) ? cardMessageResponse.Buttons[j].Text : null,
                                        Uri = new Uri(cardMessageResponse.Buttons[j].Postback)
                                    }
                                }
                            };
                        }
                        else
                        {
                            options[j] = new DocumentSelectOption
                            {
                                Label = new DocumentContainer
                                {
                                    Value = new PlainText
                                    {
                                        Text = !string.IsNullOrEmpty(cardMessageResponse.Buttons[j].Text) ? cardMessageResponse.Buttons[j].Text : null
                                    }
                                },
                                Value = new DocumentContainer
                                {
                                    Value = new PlainText
                                    {
                                        Text = cardMessageResponse.Buttons[j].Postback
                                    }
                                }
                            };
                        }
                    }
                }
            }

            return options;
        }

        #endregion

        #region IBlipAiMessageParse Members

        public Task<IList<Document>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            DocumentCollection documentCollection = null;

            var cardMessageCollection = queryResponse.ToCards();

            if (cardMessageCollection != null)
            {
                documentCollection = new DocumentCollection
                {
                    ItemType = DocumentSelect.MediaType,
                    Items = new DocumentSelect[cardMessageCollection.Count],
                    Total = cardMessageCollection.Count
                };

                for (int i = 0; i < cardMessageCollection.Count; i++)
                {
                    documentCollection.Items[i] = new DocumentSelect
                    {
                        Header = GetHeader(cardMessageCollection[i]),
                        Options = GetOptions(cardMessageCollection[i])
                    };
                }
            }

            return Task.FromResult<IList<Document>>(new List<Document>() { documentCollection });
        }

        #endregion

    }
}
