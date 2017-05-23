using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Ai.Domain.DataTransferObject.Response;
using Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces;
using Lime.Protocol;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai
{
    public class BlipAiMessageTranslator : IBlipAiMessageTranslator
    {
        #region Private Fields

        private QueryResponse _queryResponse;

        #endregion

        #region Constructor

        public BlipAiMessageTranslator()
        {

        }

        #endregion

        #region IMessageTranslator members
        
        public Task<Document> TranslateAsync(QueryResponse queryResponse)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
