using Elisoft.Notificator.Core.Models;

namespace Elisoft.Notificator.Core.Mappers
{
  public interface IMessageModelMapper
  {
    Notification MapToNotification(MessageModel message);
  }
}