using System.Text.Json;

namespace Elisoft.Notificator.Core.Models
{
  public class MessageModel
  {
    public string Channel { get; set; }
    public JsonElement Payload { get; set; }
  }
}