using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Domain.DataTransferObject.Response.Message;
using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.Service.Extensions
{
    public static class QueryResponseExtension
    {
        #region Private Methods

        private static void IsValid(QueryResponse queryResponse)
        {
            if (queryResponse == null || queryResponse.Result == null ||
                (queryResponse.Result.Fulfillment == null || queryResponse.Result.Fulfillment.Messages.Count() == 0))
            {
                throw new InvalidCastException($"Invalid query response - Object is null or empty.");
            }
        }

        #endregion

        public static List<CardMessageResponse> ToCards(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            try
            {
                var cardMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Card);

                var cardMessageList = cardMessageCollection.ToList();

                if (cardMessageList != null && cardMessageList.Count > 0)
                {
                    var messages = new List<CardMessageResponse>();

                    foreach (var cardMessage in cardMessageList)
                    {
                        messages.Add(cardMessage as CardMessageResponse);
                    }

                    return messages;
                }
            }
            catch { }

            return null;
        }

        public static List<ImageMessageResponse> ToImages(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            try
            {
                var imageMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Image);

                var imageMessageList = imageMessageCollection.ToList();

                if (imageMessageList != null && imageMessageList.Count > 0)
                {
                    var messages = new List<ImageMessageResponse>();

                    foreach (var imageMessage in imageMessageList)
                    {
                        messages.Add(imageMessage as ImageMessageResponse);
                    }

                    return messages;
                }

            }
            catch { }

            return null;
        }

        public static List<PayloadMessageResponse> ToPayloads(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            try
            {
                var payloadMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Payload);

                var payloadMessageList = payloadMessageCollection.ToList();

                if (payloadMessageList != null && payloadMessageList.Count > 0)
                {
                    var messages = new List<PayloadMessageResponse>();

                    foreach (var payloadMessage in payloadMessageList)
                    {
                        messages.Add(payloadMessage as PayloadMessageResponse);
                    }

                    return messages;
                }

            }
            catch { }

            return null;
        }

        public static List<QuickReplyMessageResponse> ToQuickReplaies(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            try
            {
                var quickReplayMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.QuickReply);

                var quickReplyMessageList = quickReplayMessageCollection.ToList();

                if (quickReplyMessageList != null && quickReplyMessageList.Count > 0)
                {
                    var messages = new List<QuickReplyMessageResponse>();

                    foreach (var cardMessage in quickReplyMessageList)
                    {
                        messages.Add(cardMessage as QuickReplyMessageResponse);
                    }

                    return messages;
                }
            }
            catch { }

            return null;
        }

        public static List<TextMessageResponse> ToTexts(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            try
            {
                var textMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Text).ToList();

                var textMessageList = textMessageCollection.ToList();

                if (textMessageList != null && textMessageList.Count > 0)
                {
                    var messages = new List<TextMessageResponse>();

                    foreach (var textMessage in textMessageList)
                    {
                        messages.Add(textMessage as TextMessageResponse);
                    }

                    return messages;
                }
            }
            catch { }

            return null;
        }

        public static List<Api.Ai.Domain.Enum.Type> ToOrderedMessageTypes(this QueryResponse queryResponse)
        {
            var types = new List<Api.Ai.Domain.Enum.Type>();

            foreach (var message in queryResponse.Result.Fulfillment.Messages)
            {
                var type = (Api.Ai.Domain.Enum.Type)message.Type;

                if (message.Type == (int)Api.Ai.Domain.Enum.Type.Card || message.Type == (int)Api.Ai.Domain.Enum.Type.QuickReply)
                {
                    if (!types.Contains(type))
                    {
                        types.Add(type);
                    }
                }
                else
                {
                    types.Add(type);
                }
            }

            return types;
        }

        public static string ToFileExtension(this string imageUrl)
        {
            var parameters = imageUrl.Split('.');

            if (parameters != null && parameters.Count() > 0)
            {
                var i = parameters.Count() - 1;
                return parameters[i];
            }

            return null;
        }

        public static string ToMediaType(this string imageUrl)
        {
            var fileExtension = ToFileExtension(imageUrl);
            return MimeTypeMap.GetMimeType(fileExtension);

        }

    }
}
