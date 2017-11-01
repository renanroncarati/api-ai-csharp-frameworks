using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
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
        public static void IsValid(QueryResponse queryResponse)
        {
            if (queryResponse == null || queryResponse.Result == null ||
                (queryResponse.Result.Fulfillment == null || queryResponse.Result.Fulfillment.Messages.Count() == 0))
            {
                throw new InvalidCastException($"Invalid query response - Object is null or empty.");
            }
        }

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

        public static List<MessageDescriptor> ToMessageDescriptors(this QueryResponse queryResponse)
        {
            var result = new List<MessageDescriptor>();

            for (int i = 0; i < queryResponse.Result.Fulfillment.Messages.Count(); i++)
            {
                var type = (Api.Ai.Domain.Enum.Type)queryResponse.Result.Fulfillment.Messages[i].Type;

                if (queryResponse.Result.Fulfillment.Messages[i].Type == (int)Api.Ai.Domain.Enum.Type.Card ||
                    queryResponse.Result.Fulfillment.Messages[i].Type == (int)Api.Ai.Domain.Enum.Type.QuickReply)
                {
                    if (result.Where(x => x.Type == type).Count() == 0)
                    {
                        result.Add(new MessageDescriptor
                        {
                            Index = i,
                            Type = type
                        });
                    }
                }
                else
                {
                    result.Add(new MessageDescriptor
                    {
                        Index = i,
                        Type = type
                    });
                }
            }

            return result;
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
