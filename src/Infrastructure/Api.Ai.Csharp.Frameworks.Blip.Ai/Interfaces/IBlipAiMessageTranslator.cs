using Api.Ai.Domain.DataTransferObject.Response;
using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Blip.Ai.Interfaces
{
    public interface IBlipAiMessageTranslator
    {
        Task<IList<Document>> TranslateAsync(QueryResponse queryResponse);
    }
}
