// Developed by Softeq Development Corporation
// http://www.softeq.com

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Softeq.NetKit.Profile.Domain.Models.PushNotification
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DevicePlatformEnum
    {
        iOS = 0,
        Android = 1
    }
}