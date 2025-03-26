namespace Boilerplate.Beta.Core.Infrastructure
{
    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; }
    }

    public class LogLevelSettings
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }
}