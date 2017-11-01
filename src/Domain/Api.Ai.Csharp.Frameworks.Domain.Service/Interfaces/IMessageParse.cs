using Api.Ai.Csharp.Frameworks.Domain.DataTransferObject;
using Api.Ai.Domain.DataTransferObject.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces
{
    public interface IMessageParse<T> where T : class 
    {
        Task<T> ParseAsync(QueryResponse queryResponse, MessageDescriptor messageDescriptor);
    }
}
