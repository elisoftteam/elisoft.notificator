using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Core.Requests;
using Elisoft.Notificator.Twilio.Services; 
using Microsoft.Extensions.Logging;
using Paramore.Brighter;
using System.Threading;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Core.Handlers
{
    public class TwilioNotificationRequestHandler : RequestHandlerAsync<TwilioNotificationRequest>
    {
        private readonly ITwilioNotificator _twilioNotificator;
        private readonly IConfig _config;
        private readonly ILogger<TwilioNotificationRequestHandler> _logger;

        public TwilioNotificationRequestHandler(
            ITwilioNotificator twilioNotificator,
            IConfig config,
            ILogger<TwilioNotificationRequestHandler> logger)
        {
            _twilioNotificator = twilioNotificator;
            _config = config;
            _logger = logger;
        }

        public override async Task<TwilioNotificationRequest> HandleAsync(TwilioNotificationRequest command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Sending SMS via Twilio.");

            await _twilioNotificator.SendSmsAsync(
                _config.TwilioAccountSid,
                _config.TwilioAuthToken,
                _config.TwilioFromNumber,
                command.To,
                command.Message
            );

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}