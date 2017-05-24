using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Api.Ai.Csharp.Frameworks.Domain.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlatformAttachmentType
    {
        [EnumMember(Value = "video")]
        Video = 0,
        [EnumMember(Value = "file")]
        File = 1,
        [EnumMember(Value = "audio")]
        Audio = 2
    }
}
