using Elisoft.Notificator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Core.Factories
{
    public interface INotificationDtoFactory
    {
        NotificationDto CreateFrom(JsonElement jsonBody);
    }
}
