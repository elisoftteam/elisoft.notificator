using Paramore.Brighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elisoft.Notificator.Slack.Request;
using Microsoft.Extensions.Logging;
using Elisoft.Notificator.Slack.Interfaces;

namespace Elisoft.Notificator.Slack.Handlers
{
    public class SlackNotificationRequestHandler:RequestHandlerAsync<SlackNotificationRequest>
    {
        private readonly ISlackNotificationService _slackService;
        private readonly ILogger<SlackNotificationRequestHandler> _logger;

        public SlackNotificationRequestHandler(
            ISlackNotificationService slackService,
            ILogger<SlackNotificationRequestHandler> logger)
        {
            _slackService = slackService;
            _logger = logger;
        }
        public override async Task<SlackNotificationRequest> HandleAsync(SlackNotificationRequest command, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Handling SlackNotificationRequest with Message: {Message}", command.Message);
            var result = await _slackService.SendMessageAsync(command.Message);
            if (!result)
            {
                _logger.LogError("Failed to send message to Slack.");
            }
            return await base.HandleAsync(command, cancellationToken);
        }
    }
}
