﻿using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Domain.DataTransferObject.Response.Message;
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

            var cardMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Card);

            if (cardMessageCollection != null)
            {
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

            return null;
        }

        public static List<ImageMessageResponse> ToImages(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            var imageMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Image);

            if (imageMessageCollection != null)
            {
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

            return null;
        }

        public static List<PayloadMessageResponse> ToPayloads(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            var payloadMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Payload);

            if (payloadMessageCollection != null)
            {
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

            return null;
        }

        public static List<QuickReplyMessageResponse> ToQuickReplaies(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            var quickReplayMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.QuickReply);

            if (quickReplayMessageCollection != null)
            {
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

            return null;
        }

        public static List<TextMessageResponse> ToTexts(this QueryResponse queryResponse)
        {
            IsValid(queryResponse);

            var textMessageCollection = queryResponse.Result.Fulfillment.Messages.Where(x => x.Type == (int)Api.Ai.Domain.Enum.Type.Text);

            if (textMessageCollection != null)
            {
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

            return null;
        }

    }
}
