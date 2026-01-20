using Elisoft.Notificator.Core.Enums;
using Elisoft.Notificator.Core.Models;
using System.Text.Json;
using Elisoft.Notificator.Api.Contracts;

namespace Elisoft.Notificator.Api.Mappers
{

    public class MessageMapper : IMessageMapper
    {
        public Notification MapToNotification(Message message)
        {
            if (message == null) throw new ArgumentException("Body cannot be empty.");

            if (string.IsNullOrWhiteSpace(message.Channel))
                throw new ArgumentException("Missing 'channel' property.");

            if (!Enum.TryParse<NotificationEnumChannel>(message.Channel, true, out var channel))
            {
                var validChannels = string.Join(", ", Enum.GetNames(typeof(NotificationEnumChannel)));
                throw new ArgumentException($"Invalid channel value: {message.Channel}. Available: {validChannels}");
            }

            if (message.Payload.ValueKind == JsonValueKind.Undefined || message.Payload.ValueKind == JsonValueKind.Null)
                throw new ArgumentException("Missing 'payload' property.");

            return new Notification
            {
                Channel = channel,
                Payload = message.Payload
            };
        }
    }
}