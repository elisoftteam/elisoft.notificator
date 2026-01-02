using Elisoft.Notificator.Core.Enums;
using Elisoft.Notificator.Core.Models;
using System.Text.Json;

namespace Elisoft.Notificator.Core.Factories
{
    public class NotificationDtoFactory : INotificationDtoFactory
    {
        public NotificationDto CreateFrom(JsonElement jsonBody)
        {
            if (jsonBody.ValueKind == JsonValueKind.Undefined || jsonBody.ValueKind == JsonValueKind.Null)
            {
                throw new ArgumentException("Body cannot be empty.");
            }

            if (!jsonBody.TryGetProperty("channel", out var channelElement))
            {
                throw new ArgumentException("Missing 'channel' property.");
            }

            var channelString = channelElement.GetString();
            if (!Enum.TryParse<NotificationEnumChannel>(channelString, true, out var channel))
            {
                var validChannels = string.Join(", ", Enum.GetNames(typeof(NotificationEnumChannel)));
                throw new ArgumentException($"Invalid channel value: {channelString}. Available: {validChannels}");
            }

            if (!jsonBody.TryGetProperty("payload", out var payloadElement))
            {
                throw new ArgumentException("Missing 'payload' property.");
            }

            return new NotificationDto
            {
                Channel = channel,
                Payload = payloadElement
            };
        }
    }
}