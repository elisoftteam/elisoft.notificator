using System.Text.Json;

namespace Elisoft.Notificator.Api.Contracts
{
  public class Message
  {
    public string Channel { get; set; }
    public JsonElement Payload { get; set; }
  }
}