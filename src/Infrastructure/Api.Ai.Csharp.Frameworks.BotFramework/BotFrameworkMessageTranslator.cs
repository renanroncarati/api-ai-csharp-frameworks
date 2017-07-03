using Api.Ai.Csharp.Frameworks.BotFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Domain.DataTransferObject.Response;
using Microsoft.Bot.Connector;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Csharp.Frameworks.BotFramework.Extensions;
using Newtonsoft.Json;
using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;

namespace Api.Ai.Csharp.Frameworks.BotFramework
{
    public class BotFrameworkMessageTranslator : IBotFrameworkMessageTranslator
    {
        #region Private Methods

        private Activity GetCardMessage(QueryResponse queryResponse, Activity message)
        {
            Activity activity = null;

            var cardImages = new List<CardImage>();
            var cardActions = new List<CardAction>();

            var cardMessageCollection = queryResponse.ToCards();

            if (cardMessageCollection != null)
            {
                activity = message.CreateReply();
                activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var cardMessage in cardMessageCollection)
                {
                    cardImages.Add(new CardImage
                    {
                        Url = cardMessage.ImageUrl
                    });

                    var cardButtons = new List<CardAction>();

                    foreach (var button in cardMessage.Buttons)
                    {
                        cardButtons.Add(new CardAction
                        {
                            Title = button.Text,
                            Type = button.ToCardActionType(),
                            Value = button.Postback
                        });
                    }

                    var heroCard = new HeroCard()
                    {
                        Title = cardMessage.Title,
                        Subtitle = cardMessage.Subtitle,
                        Images = cardImages,
                        Buttons = cardButtons
                    };

                    Attachment attachment = heroCard.ToAttachment();
                    activity.Attachments.Add(attachment);
                }
            }



            return activity;
        }

        private Activity GetImageMessages(QueryResponse queryResponse, Activity message)
        {
            var imageMessageCollection = queryResponse.ToImages();

            if (imageMessageCollection != null)
            {
                var replyMessage = message.CreateReply();

                foreach (var imageMessage in imageMessageCollection)
                {
                    if (!string.IsNullOrEmpty(imageMessage.ImageUrl))
                    {
                        replyMessage.Attachments.Add(new Attachment()
                        {
                            ContentUrl = imageMessage.ImageUrl,
                            ContentType = imageMessage.ImageUrl.ToMediaType(),
                            ThumbnailUrl = imageMessage.ImageUrl
                        });
                    }
                }

                return replyMessage;
            }

            return null;
        }

        private Activity GetPayloadMessages(QueryResponse queryResponse, Activity message)
        {
            Activity activity = null;

            var payloadMessageCollection = queryResponse.ToPayloads();

            if (payloadMessageCollection != null)
            {
                activity = message.CreateReply();

                foreach (var payloadMessage in payloadMessageCollection)
                {
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
            }

            return activity;
        }

        private List<Activity> GetQuickReplayMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> activities = null;

            var quickReplayCollection = queryResponse.ToQuickReplaies();

            if (quickReplayCollection != null)
            {
                activities = new List<Activity>();

                foreach (var quickReplay in quickReplayCollection)
                {
                    var activity = message.CreateReply();
                    activity.Text = quickReplay.Title;
                    activity.Type = ActivityTypes.Message;
                    activity.TextFormat = TextFormatTypes.Plain;

                    activity.SuggestedActions = new SuggestedActions();
                    activity.SuggestedActions.Actions = new List<CardAction>();

                    if (quickReplay.Replies != null)
                    {
                        foreach (var reply in quickReplay.Replies)
                        {
                            activity.SuggestedActions.Actions.Add(new CardAction() { Title = reply, Type = ActionTypes.ImBack, Value = reply });
                        }
                    }

                    activities.Add(activity);
                }
            }
            return activities;
        }

        private List<Activity> GetTextMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> activities = null;

            var textMessageCollection = queryResponse.ToTexts();

            if (textMessageCollection != null)
            {
                activities = new List<Activity>();

                foreach (var textMessage in textMessageCollection)
                {
                    if (!string.IsNullOrEmpty(textMessage.Speech))
                    {
                        var replyMessage = message.CreateReply();
                        replyMessage.Text = textMessage.Speech;
                        activities.Add(replyMessage);
                    }
                }
            }

            return activities;
        }

        #endregion

        #region IBotFrameworkMessageTranslator Members

        public Task<IList<Activity>> TranslateAsync(QueryResponse queryResponse, Activity message)
        {
            var activities = new List<Activity>();

            var textMessages = GetTextMessages(queryResponse, message);

            if (textMessages != null && textMessages.Count > 0)
            {
                foreach (var text in textMessages)
                {
                    activities.Add(text);
                }
            }

            var cardMessage = GetCardMessage(queryResponse, message);

            if (cardMessage != null)
            {
                activities.Add(cardMessage);
            }

            var imageMessage = GetImageMessages(queryResponse, message);

            if (imageMessage != null)
            {
                activities.Add(imageMessage);

            }

            var payloadMessage = GetPayloadMessages(queryResponse, message);

            if (payloadMessage != null)
            {
                activities.Add(payloadMessage);
            }

            var quickReplayMessages = GetQuickReplayMessages(queryResponse, message);

            if (quickReplayMessages != null)
            {
                activities.AddRange(quickReplayMessages);

            }

            return Task.FromResult<IList<Activity>>(activities);
        }

        #endregion
    }
}
