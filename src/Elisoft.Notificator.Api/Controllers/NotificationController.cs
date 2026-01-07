using Elisoft.Notificator.Core.Mappers;
using Elisoft.Notificator.Core.Models;
using Elisoft.Notificator.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Elisoft.Notificator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMessageModelMapper _messageMapper;

        public NotificationController(
            INotificationService notificationService,
            IMessageModelMapper messageMapper)
        {
            _notificationService = notificationService;
            _messageMapper = messageMapper;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] MessageModel message)
        {
            try
            {
                var notification = _messageMapper.MapToNotification(message);

                await _notificationService.DispatchNotificationAsync(notification);

                return Ok(new { Status = "Success", Channel = notification.Channel.ToString() });
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Validation Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Error: {ex.Message}");
            }
        }
    }
}