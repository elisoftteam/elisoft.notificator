using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Requests
{
    public class TwilioNotificationRequest : Command
    {
        public TwilioNotificationRequest() : base(Guid.NewGuid())
        {
        }

        public string To { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}