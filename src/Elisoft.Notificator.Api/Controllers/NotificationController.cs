using Elisoft.Notificator.Core.Factories;
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
    private readonly IMessageModelFactory _messageFactory;

    public NotificationController(
        INotificationService notificationService,
        IMessageModelFactory messageFactory)
    {
      _notificationService = notificationService;
      _messageFactory = messageFactory;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] MessageModel message)
    {
      try
      {
        var notificationDto = _messageFactory.CreateFrom(message);

        await _notificationService.DispatchNotificationAsync(notificationDto);

        return Ok(new { Status = "Success", Channel = notificationDto.Channel.ToString() });
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