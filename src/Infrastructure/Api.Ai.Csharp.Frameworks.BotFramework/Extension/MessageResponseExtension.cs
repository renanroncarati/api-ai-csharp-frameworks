using Api.Ai.Domain.DataTransferObject.Response.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Extension
{
    public static class MessageResponseExtension
    {
        public static string ToCardActionType(this CardMessageResponseButton cardMessageResponseButton)
        {
            return !string.IsNullOrEmpty(cardMessageResponseButton.Postback) && cardMessageResponseButton.Postback.Contains("http") ? "openUrl" : "postBack";
        }
    }
}
