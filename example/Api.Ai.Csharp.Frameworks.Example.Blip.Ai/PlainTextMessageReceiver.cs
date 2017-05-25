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

namespace Api.Ai.Csharp.Frameworks.Example.Blip.Ai
{
    public class PlainTextMessageReceiver : IMessageReceiver
    {
        #region Private fields

        private readonly IApiAiAppServiceFactory _apiAiAppServiceFactory;
        private readonly IBlipAiMessageTranslator _blipAiMessageTranslator;
        private readonly IMessagingHubSender _sender;

        #endregion

        public PlainTextMessageReceiver(IMessagingHubSender sender, IApiAiAppServiceFactory apiAiAppServiceFactory,
            IBlipAiMessageTranslator blipAiMessageTranslator)
        {
            _apiAiAppServiceFactory = apiAiAppServiceFactory;
            _blipAiMessageTranslator = blipAiMessageTranslator;
            _sender = sender;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            var queryAppService = _apiAiAppServiceFactory.CreateQueryAppService("https://api.api.ai/v1", "543b9445d21a4d1bb79c3f569f8d4827");

            var queryRequest = new QueryRequest
            {
                SessionId = "1",
                Query = new string[] { message.Content.ToString() },
                Lang = Api.Ai.Domain.Enum.Language.Portuguese
            };

            var queryResponse = await queryAppService.PostQueryAsync(queryRequest);

            var documents = await _blipAiMessageTranslator.TranslateAsync(queryResponse);

            foreach (var document in documents)
            {
                await _sender.SendMessageAsync(document, message.From, cancellationToken);
            }
        }
    }
}
