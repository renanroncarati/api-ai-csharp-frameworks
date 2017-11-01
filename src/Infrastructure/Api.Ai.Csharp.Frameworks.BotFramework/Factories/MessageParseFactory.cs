using Api.Ai.Csharp.Frameworks.BotFramework.Parse;
using Api.Ai.Csharp.Frameworks.Domain.Service.Factories;
using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Factories
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
                    return _serviceProvider.GetService(typeof(BotFrameworkTextMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Card:
                    return _serviceProvider.GetService(typeof(BotFrameworkCardMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.QuickReply:
                    return _serviceProvider.GetService(typeof(BotFrameworkQuickReplyMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Image:
                    return _serviceProvider.GetService(typeof(BotFrameworkImageMessageParse)) as IMessageParse<T>;

                case Api.Ai.Domain.Enum.Type.Payload:
                    return _serviceProvider.GetService(typeof(BotFrameworkPayloadMessageParse)) as IMessageParse<T>;

                default:
                    throw new NotImplementedException($"IMessageParse is not implemented for type: {type}");
            }
        }

        #endregion

    }
}
