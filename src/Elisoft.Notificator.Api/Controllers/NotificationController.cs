using Elisoft.Notificator.Slack.Interfaces;
using Elisoft.Notificator.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Paramore.Brighter;
using System.Text.Json;

namespace Elisoft.Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IAmACommandProcessor _commandProcessor;
    public NotificationController(ILogger<NotificationController> logger, IAmACommandProcessor commandProcessor)
        {
            _logger = logger;
            _commandProcessor = commandProcessor;
        }
        [HttpPost("slack")]
        public async Task<IActionResult> DispatchNotification([FromBody] NotificationDto request)
        {
            if (request == null) {
                return BadRequest("Request body cannot be null.");

            }
            try
            {
                var requestTypeName = $"{request.Channel}NotificationRequest";
                var targetType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .FirstOrDefault(t => t.Name.Equals(requestTypeName, StringComparison.InvariantCultureIgnoreCase)
                                 && !t.IsInterface
                                 && !t.IsAbstract);
                if (targetType == null)
                {
                    _logger.LogError("No matching notification request type found for channel: {Channel}", request.Channel);
                    return BadRequest($"Unsupported notification channel: {request.Channel}");
                }
                var command = JsonSerializer.Deserialize(request.Payload.GetRawText(), targetType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                _logger.LogInformation("Dispatching notification command of type: {CommandType}", targetType.Name);
                await ((dynamic)_commandProcessor).SendAsync((dynamic)command);
                return Ok(new { status = "Succes", channel = request.Channel.ToString(), targetType.Name });
            
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Dispatcher error");
                return StatusCode(500, ex.Message);
            }
            

        }
    }
}