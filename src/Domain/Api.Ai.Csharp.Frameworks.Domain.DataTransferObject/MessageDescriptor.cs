using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.DataTransferObject
{
    public class MessageDescriptor
    {
        public int Index { get; set; }

        public Api.Ai.Domain.Enum.Type Type { get; set; }
    }
}
