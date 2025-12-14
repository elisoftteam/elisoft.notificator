using System;
using System.IO;
using Elisoft.Notificator.Configuration.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Elisoft.Notificator.Configuration.Configuration
{
  public class LoggingConfig
  {
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {


      Log.Logger = new LoggerConfiguration()
     .ReadFrom.Configuration(configuration)
          .CreateLogger();

      services.AddLogging(loggingBuilder =>
      {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(dispose: true);
      });
    }
  }
}