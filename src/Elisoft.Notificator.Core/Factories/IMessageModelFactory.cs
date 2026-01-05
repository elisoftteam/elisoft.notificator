using Elisoft.Notificator.Core.Models;

namespace Elisoft.Notificator.Core.Factories
{
  public interface IMessageModelFactory
  {
    NotificationDto CreateFrom(MessageModel message);
  }
}