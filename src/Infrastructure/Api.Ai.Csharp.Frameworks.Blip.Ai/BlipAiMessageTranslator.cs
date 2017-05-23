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
                    documentCollection.Items[i] = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Type = MediaType.Parse("image/jpeg"),
                            PreviewType = new MediaType(MediaType.DiscreteTypes.Image, MediaType.SubTypes.JPeg),
                            PreviewUri = new Uri(imageMessageCollection[i].ImageUrl),
                            Uri = new Uri(imageMessageCollection[i].ImageUrl)
                        }
                    };
                }
            }

            return documentCollection;
        }

        private Document GetPayloadMessages(QueryResponse queryResponse)
        {
            DocumentCollection documentCollection = null;

            var payloadMessageCollection = queryResponse.ToPayloads();

            return documentCollection;
        }

        private Document GetQuickReplayMessages(QueryResponse queryResponse)
        {
            DocumentCollection documentCollection = null;

            var quickReplayMessageCollection = queryResponse.ToQuickReplaies();

            return documentCollection;
        }

        private Document GetTextMessages(QueryResponse queryResponse)
        {
            DocumentCollection documentCollection = null;

            var textMessageCollection = queryResponse.ToTexts();

            return documentCollection;
        }

        #endregion

        #region IMessageTranslator members

        public Task<IList<Document>> TranslateAsync(QueryResponse queryResponse)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
