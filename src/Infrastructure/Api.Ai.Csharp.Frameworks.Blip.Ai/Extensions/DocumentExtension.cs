using Lime.Messaging.Contents;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Takenet.MessagingHub.Client.Sender;
using Takenet.MessagingHub.Client;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Extensions
{
    public static class DocumentExtension
    {
        /// <summary>
        /// Send chat state message
        /// </summary>
        /// <param name="document"></param>
        /// <param name="sender"></param>
        /// <param name="to"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="index">Current index in document collection</param>
        /// <param name="length">Document collection length</param>
        /// <returns></returns>
        public async static Task SendChatStateAsync(this Document document, IMessagingHubSender sender, string to, CancellationToken cancellationToken, int index, int length)
        {
            if (index < length - 1)
            {
                await sender.SendMessageAsync(new ChatState { State = ChatStateEvent.Composing }, to, cancellationToken);
                await ChatStateDelayAsync(document);
            }
        }

        #region Private Methods

        private async static Task ChatStateDelayAsync(Document document)
        {
            if (document != null)
            {
                if (document is PlainText)
                {
                    var plainText = document as PlainText;

                    if (!string.IsNullOrEmpty(plainText))
                    {
                        await Task.Delay(plainText.Text.Length * 35);
                    }
                }
            }

            await Task.Delay(650);
        }
        
        #endregion

    }
}
