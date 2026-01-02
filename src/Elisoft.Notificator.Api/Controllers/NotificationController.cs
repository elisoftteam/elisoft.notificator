using Elisoft.Notificator.Core.Factories;
using Elisoft.Notificator.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Elisoft.Notificator.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly INotificationDtoFactory _dtoFactory;

        public NotificationController(
            INotificationService notificationService,
            INotificationDtoFactory dtoFactory)
        {
            _notificationService = notificationService;
            _dtoFactory = dtoFactory;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] JsonElement jsonBody)
        {
            try
            {
                var notificationDto = _dtoFactory.CreateFrom(jsonBody);

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