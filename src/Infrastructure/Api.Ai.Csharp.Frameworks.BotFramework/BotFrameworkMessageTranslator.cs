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
            activity = message.CreateReply();
            activity.AttachmentLayout = "carousel";
            activity.Attachments = new List<Attachment>();

            var cardImages = new List<CardImage>();
            var cardActions = new List<CardAction>();

            var cardMessageCollection = queryResponse.ToCards();

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

            return activity;
        }

        private List<Activity> GetImageMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> result = null;

            var imageMessageCollection = queryResponse.ToImages();

            if (imageMessageCollection != null)
            {

            }

            return result;
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

        private List<Activity> GetQuickReplayMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> activities = null;

            var quickReplayMessageCollection = queryResponse.ToQuickReplaies();

            if (quickReplayMessageCollection != null)
            {

            }

            return activities;
        }

        private List<Activity> GetTextMessages(QueryResponse queryResponse, Activity message)
        {
            List<Activity> activities = null;

            var textMessageCollection = queryResponse.ToTexts();

            if (textMessageCollection != null)
            {

            }

            return activities;
        }

        #endregion

        #region IBotFrameworkMessageTranslator Members

        public Task<IList<Activity>> TranslateAsync(QueryResponse queryResponse, Activity message)
        {
            var activities = new List<Activity>();

            var cardMessage = GetCardMessage(queryResponse, message);

            if (cardMessage != null)
            {
                activities.Add(cardMessage);
            }

            var imageMessages = GetImageMessages(queryResponse, message);

            if (imageMessages != null && imageMessages.Count > 0)
            {
                foreach (var image in imageMessages)
                {
                    activities.Add(image);
                }
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

            if (quickReplayMessages != null && quickReplayMessages.Count > 0)
            {
                foreach (var quickReplay in quickReplayMessages)
                {
                    activities.Add(quickReplay);
                }
            }

            var textMessages = GetTextMessages(queryResponse, message);

            if (textMessages != null && textMessages.Count > 0)
            {
                foreach (var text in textMessages)
                {
                    activities.Add(text);
                }
            }

            return Task.FromResult<IList<Activity>>(activities);
        }

        #endregion

    }
}
