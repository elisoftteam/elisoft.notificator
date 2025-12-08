using elisoft.notification.Configuration.Configuration;
using elisoft.notification.Core.Interfaces;
using elisoft.notification.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace elisoft.notification.Infrastructure.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            LoggingConfig.ConfigureServices(services, configuration);
            services.AddHttpClient<ISlackNotificationService, SlackNotificationService>();

            return services;
        }
    }
}