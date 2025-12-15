using Elisoft.Notificator.Configuration.Configuration;
using Elisoft.Notificator.Slack.Handlers;
using Elisoft.Notificator.Slack.Interfaces;
using Elisoft.Notificator.Slack.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Elisoft.Notificator.Infrastructure.Dependencies
{
  public static class DependencyInjection
  {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      LoggingConfig.ConfigureServices(services, configuration);
      services.AddSingleton<IConfig, Config>();
      services.AddHttpClient<ISlackNotificationService, SlackNotificationService>();
            services.AddBrighter()
                  .AutoFromAssemblies(new[] { typeof(SlackNotificationRequestHandler).Assembly });
            return services;
    }
  }
}