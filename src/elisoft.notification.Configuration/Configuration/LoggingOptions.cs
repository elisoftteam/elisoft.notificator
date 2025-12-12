namespace elisoft.notification.Configuration.Configuration
{
    public class LoggingOptions
    {
        public FileSettings File { get; set; } = new FileSettings();
    }

    public class FileSettings
    {
        public string LogsDirectory { get; set; } = string.Empty;
    }
}