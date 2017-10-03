using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using Takenet.MessagingHub.Client;
using Takenet.MessagingHub.Client.Listener;
using Takenet.MessagingHub.Client.Sender;
using System.Diagnostics;
using Api.Ai.ApplicationService.Factories;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Api.Ai.Domain.DataTransferObject.Request;
using Lime.Messaging.Contents;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Extensions;

namespace Api.Ai.Csharp.Frameworks.Example.Blip.Ai
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        #region Private fields

        private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
        private readonly IBlipAiMessageTranslator _blipAiMessageTranslator;
        private readonly IMessagingHubSender _sender;
        private readonly ExampleSettings _settings;

        #endregion

        public PlainTextMessageReceiver(IMessagingHubSender sender, IApiAiAppServiceFactory apiAiAppServiceFactory,
            IBlipAiMessageTranslator blipAiMessageTranslator, ExampleSettings settings)
        {
            _apiAiAppServiceFactory = apiAiAppServiceFactory;
            _blipAiMessageTranslator = blipAiMessageTranslator;
            _sender = sender;
            _settings = settings;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", _settings.ApiAiAccessToken);

            var queryRequest = new QueryRequest
            {
                SessionId = message.From.Name.Length > 36 ? message.From.Name.Substring(0, 35) : message.From.Name,
                Query = new string[] { message.Content.ToString() },
                Lang = Api.Ai.Domain.Enum.Language.BrazilianPortuguese
            };

            var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

            var documents = await _blipAiMessageTranslator.TranslateAsync(queryResponse);

            for (int i = 0; i < documents.Count; i++)
            {
                await _sender.SendMessageAsync(documents[i], message.From, cancellationToken);
                await documents[i].SendChatStateAsync(_sender, message.From, cancellationToken, i, documents.Count);
            }
        }
    }
}
