using Api.Ai.Csharp.Frameworks.Domain.Service.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using Api.Ai.Domain.Enum;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Parse;
using Lime.Protocol;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Factories
{
    public class MessageParseFactory : IMessageParseFactory
    {
        #region Private Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructor

        public MessageParseFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region IMessageParseFactory Members

        public IMessageParse<T> Create<T>(Api.Ai.Domain.Enum.Type type) where T : class
        {
            switch (type)
            {
                case Api.Ai.Domain.Enum.Type.Text:
                    return _serviceProvider.GetService(typeof(BlipAiTextMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Card:
                    return _serviceProvider.GetService(typeof(BlipAiCardMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.QuickReply:
                    return _serviceProvider.GetService(typeof(BlipAiQuickReplyMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Image:
                    return _serviceProvider.GetService(typeof(BlipAiImageMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Payload:
                    return _serviceProvider.GetService(typeof(BlipAiPayloadMessageParse)) as IMessageParse<T>;

                default:
                    throw new NotImplementedException($"IMessageParse is not implemented for type: {type}");
            }
        }

        #endregion

    }
}
