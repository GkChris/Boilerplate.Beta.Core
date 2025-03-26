namespace Boilerplate.Beta.Core.Infrastructure
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public List<string> Topics { get; set; }
        public string GroupId { get; set; }
    }
}