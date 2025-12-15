using Paramore.Brighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elisoft.Notificator.Slack.Request
{
    public class SlackNotificationRequest:Command
    {
        public SlackNotificationRequest():base(Guid.NewGuid())
        {

        }
        public string Message { get; set; } = string.Empty;
    }
}
