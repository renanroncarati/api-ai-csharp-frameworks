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

namespace Api.Ai.Csharp.Frameworks.BotFramework.Parse
{
    public class BotFrameworkCardMessageParse : IMessageParse<IList<Activity>>
    {
        #region IMessageParse Members

        public Task<IList<Activity>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            Activity activity = null;

            var cardActions = new List<CardAction>();

            var cardMessageCollection = queryResponse.ToCards();

            if (cardMessageCollection != null)
            {
                activity = new Activity();
                activity.Attachments = new List<Attachment>();
                activity.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var cardMessage in cardMessageCollection)
                {
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
                        Images = new List<CardImage>()
                        {
                            new CardImage
                            {
                                Url = cardMessage.ImageUrl
                            }
                        },
                        Buttons = cardButtons
                    };

                    Attachment attachment = heroCard.ToAttachment();
                    activity.Attachments.Add(attachment);
                }
            }

            return Task.FromResult<IList<Activity>>(new List<Activity>() { activity });
        }

        #endregion

    }
}
