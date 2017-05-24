using Api.Ai.Csharp.Frameworks.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.DataTransferObject
{
    public class Payload
    {
        public string Url { get; set; }
    }
    public class FacebookAttachment
    {
        public PlatformAttachmentType Type { get; set; }
        public Payload Payload { get; set; }
    }

    public class FacebookPayload
    {
        public FacebookAttachment Attachment { get; set; }
    }

    public class PlatformPayload
    {
        public FacebookPayload Facebook { get; set; }
    }
}
