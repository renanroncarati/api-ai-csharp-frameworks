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
    public class BotFrameworkQuickReplyMessageParse : IMessageParse<IList<Activity>>
    {
        #region IMessageParse Members

        public Task<IList<Activity>> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor)
        {
            List<Activity> activities = null;

            var quickReplayCollection = queryResponse.ToQuickReplaies();

            if (quickReplayCollection != null)
            {
                activities = new List<Activity>();

                foreach (var quickReplay in quickReplayCollection)
                {
                    var activity = new Activity();
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

            return Task.FromResult<IList<Activity>>(activities);
        }

        #endregion

    }
}
