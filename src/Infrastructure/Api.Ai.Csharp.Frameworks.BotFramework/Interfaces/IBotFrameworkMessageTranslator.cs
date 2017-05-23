using Api.Ai.Domain.DataTransferObject.Response;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.BotFramework.Interfaces
{
    public interface IBotFrameworkMessageTranslator
    {
        Task<IList<Activity>> TranslateAsync(QueryResponse queryResponse, Activity message);
    }
}
