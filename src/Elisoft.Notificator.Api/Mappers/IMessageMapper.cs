using Elisoft.Notificator.Core.Models;
using Elisoft.Notificator.Api.Contracts;

namespace Elisoft.Notificator.Api.Mappers
{
  public interface IMessageMapper
  {
    Notification MapToNotification(Message message);
  }
}