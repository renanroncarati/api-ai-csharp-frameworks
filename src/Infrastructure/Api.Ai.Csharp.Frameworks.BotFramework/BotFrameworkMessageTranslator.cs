using Api.Ai.Csharp.Frameworks.BotFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Domain.DataTransferObject.Response;
using Microsoft.Bot.Connector;
using Api.Ai.Csharp.Frameworks.Domain.Service.Extensions;
using Api.Ai.Csharp.Frameworks.BotFramework.Extension;

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
                activity.AttachmentLayout = "carousel";
                activity.Attachments = new List<Attachment>();

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
                    replyMessage.Attachments.Add(new Attachment()
                    {
                        ContentUrl = imageMessage.ImageUrl,
                        ContentType = $"image/{imageMessage.ImageUrl.ToContentType()}",
                        ThumbnailUrl = imageMessage.ImageUrl
                    });
                }

                return replyMessage;
            }

            return null;
        }

        private List<Activity> GetPayloadMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> activities = null;

            var payloadMessageCollection = queryResponse.ToPayloads();

            if (payloadMessageCollection != null)
            {

            }

            return activities;
        }

        private Activity GetQuickReplayMessages(QueryResponse queryResponse, Activity message)
        {
            Activity activities = null;

            var quickReplayCollection = queryResponse.ToQuickReplaies();

            if (quickReplayCollection != null)
            {
                activities = message.CreateReply();
                activities.Type = ActivityTypes.Message;
                activities.TextFormat = TextFormatTypes.Plain;

                activities.SuggestedActions = new SuggestedActions();
                activities.SuggestedActions.Actions = new List<CardAction>();

                foreach (var quickReplay in quickReplayCollection)
                {
                    activities.SuggestedActions.Actions.Add(new CardAction() { Title = quickReplay.Title, Type = ActionTypes.ImBack, Value = quickReplay.Replies.FirstOrDefault() });
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
                    if(!string.IsNullOrEmpty(textMessage.Speech))
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

            var payloadMessages = GetPayloadMessages(queryResponse, message);

            if (payloadMessages != null && payloadMessages.Count > 0)
            {
                foreach (var payload in payloadMessages)
                {
                    activities.Add(payload);
                }
            }
           
            var quickReplayMessages = GetQuickReplayMessages(queryResponse, message);

            if (quickReplayMessages != null)
            {
                activities.Add(quickReplayMessages);

            }

            return Task.FromResult<IList<Activity>>(activities);
        }

        #endregion

    }
}
