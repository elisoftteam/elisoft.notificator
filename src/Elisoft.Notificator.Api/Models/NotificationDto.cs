using Elisoft.Notificator.Api.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Elisoft.Notificator.Api.Models
{
    public class NotificationDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public NotificationEnumChannel Channel { get; set; }
        public JsonElement Payload { get; set; }
    }
}
