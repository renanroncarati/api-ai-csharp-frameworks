using Api.Ai.Csharp.Frameworks.Domain.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.Service.Factories
{
    public interface IMessageParseFactory
    {
        IMessageParse<T> Create<T>(Api.Ai.Domain.Enum.Type type) where T : class;
    }
}
