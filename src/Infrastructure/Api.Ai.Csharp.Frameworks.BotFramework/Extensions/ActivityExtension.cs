using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Extensions
{
    public static class ActivityExtension
    {
        /// <summary>
        /// Send typing message.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="connector"></param>
        /// <param name="index">Current index in activity collection</param>
        /// <param name="length">Activity collection length</param>
        /// <returns></returns>
        public async static Task SendTypingAsync(this Activity activity, ConnectorClient connector, int index, int length)
        {
            var typing = Activity.CreateTypingActivity();
            typing.From = activity.From;
            typing.Recipient = activity.Recipient;
            typing.Conversation = new ConversationAccount { Id = activity.Conversation.Id };
            
            if (index < length - 1)
            {
                await connector.Conversations.SendToConversationAsync((Activity)typing);
                await TypingDelayAsync(activity);
            }
        }
        
        #region Private Methods

        private async static Task TypingDelayAsync(Activity activity)
        {
            if (activity != null)
            {
                if (!string.IsNullOrWhiteSpace(activity.Text))
                {
                    await Task.Delay(activity.Text.Length * 35);
                }
            }

            await Task.Delay(300);
        }

        #endregion
    }
}
