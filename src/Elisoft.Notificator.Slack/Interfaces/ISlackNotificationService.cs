namespace Elisoft.Notificator.Slack.Interfaces
{
  public interface ISlackNotificationService
  {
    Task<bool> SendMessageAsync(string message);
  }
}