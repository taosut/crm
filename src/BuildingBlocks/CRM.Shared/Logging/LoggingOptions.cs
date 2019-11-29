namespace CRM.Shared.Logging
{
    public class LoggingOptions
    {
        public bool ConsoleEnabled { get; set; }
        public SeqOptions Seq { get; set; } = default;
        public LokiOptions Loki { get; set; } = default;
    }
}
