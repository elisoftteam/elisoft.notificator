namespace elisoft.notification.Core.Interfaces
{
    public interface ISlackNotificationService
    {
        Task<bool> SendMessageAsync(string message);
    }
}