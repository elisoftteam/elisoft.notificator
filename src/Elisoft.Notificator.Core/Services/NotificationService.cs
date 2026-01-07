using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Models;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Elisoft.Notificator.Core.Services
{
    public interface INotificationService
    { 
        Task DispatchNotificationAsync(Notification notification);
    }

    public class NotificationService : INotificationService
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IRequestFactory _requestFactory;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IAmACommandProcessor commandProcessor,
            IRequestFactory requestFactory,
            ILogger<NotificationService> logger)
        {
            _commandProcessor = commandProcessor;
            _requestFactory = requestFactory;
            _logger = logger;
        }

        public async Task DispatchNotificationAsync(Notification notification)
        {
            _logger.LogInformation($"Processing a notification for a channel: {notification.Channel}");
      
            var command = _requestFactory.CreateRequest(notification.Channel, notification.Payload);

            await ((dynamic)_commandProcessor).SendAsync((dynamic)command);
        }
    }
}