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

namespace Api.Ai.Csharp.Frameworks.Blip.Ai
{
    public class BlipAiMessageTranslator : IBlipAiMessageTranslator
    {
        #region Private Fields

        #endregion

        #region Constructor

        public BlipAiMessageTranslator()
        {

        }

        #endregion

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

        private Document GetCardMessage(QueryResponse queryResponse)
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

            return documentCollection;
        }

        private Document GetImageMessage(QueryResponse queryResponse)
        {
            DocumentCollection documentCollection = null;

            var imageMessageCollection = queryResponse.ToImages();

            if (imageMessageCollection != null)
            {
                documentCollection = new DocumentCollection
                {
                    ItemType = DocumentContainer.MediaType,
                    Items = new DocumentContainer[imageMessageCollection.Count],
                    Total = imageMessageCollection.Count,
                };

                for (int i = 0; i < imageMessageCollection.Count; i++)
                {
                    if(!string.IsNullOrEmpty(imageMessageCollection[i].ImageUrl))
                    {
                        documentCollection.Items[i] = new DocumentContainer
                        {
                            Value = new MediaLink
                            {
                                Type = MediaType.Parse(imageMessageCollection[i].ImageUrl.ToMediaType()),
                                Uri = new Uri(imageMessageCollection[i].ImageUrl)
                            }
                        };
                    }                   
                }
            }

            return documentCollection;
        }

        /// <summary>
        /// TODO: Implement payload.
        /// </summary>
        /// <param name="queryResponse"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private List<Document> GetPayloadMessages(QueryResponse queryResponse)
        {
            var documents = new List<Document>();

            var payloadMessageCollection = queryResponse.ToPayloads();

            if (payloadMessageCollection != null)
            {

                for (int i = 0; i < payloadMessageCollection.Count; i++)
                {
                    var payload = JsonConvert.DeserializeObject<PlatformPayload>(payloadMessageCollection[i].Payload.ToString());

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
            }

            return documents;
        }

        private Document GetQuickReplayMessages(QueryResponse queryResponse)
        {
            Document document = null;

            var quickReplayMessageCollection = queryResponse.ToQuickReplaies();

            if (quickReplayMessageCollection != null)
            {
                var select = new Select
                {
                    Scope = SelectScope.Immediate,
                    Options = new SelectOption[quickReplayMessageCollection.Count]
                };

                for (int i = 0; i < quickReplayMessageCollection.Count; i++)
                {
                    select.Options[i] = new SelectOption
                    {
                        Text = quickReplayMessageCollection[i].Title,
                        Order = i,
                        Value = new PlainText { Text = quickReplayMessageCollection[i].Replies != null ? quickReplayMessageCollection[i].Replies.FirstOrDefault() : null }
                    };
                }
            }

            return document;
        }

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

        public Task<IList<Document>> TranslateAsync(QueryResponse queryResponse)
        {
            var documents = new List<Document>();

            var cardMessage = GetCardMessage(queryResponse);

            if (cardMessage != null)
            {
                documents.Add(cardMessage);
            }

            var imageMessage = GetImageMessage(queryResponse);

            if (imageMessage != null)
            {
                documents.Add(imageMessage);
            }

            var payloadMessages = GetPayloadMessages(queryResponse);

            if (payloadMessages != null)
            {
                documents.AddRange(payloadMessages);
            }

            var quickReplayMessage = GetQuickReplayMessages(queryResponse);

            if (quickReplayMessage != null)
            {
                documents.Add(quickReplayMessage);
            }

            var textMessages = GetTextMessages(queryResponse);

            if (textMessages != null)
            {
                documents.AddRange(textMessages);
            }

            return Task.FromResult<IList<Document>>(documents);
        }

        #endregion

    }
}
