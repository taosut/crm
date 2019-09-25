namespace CRM.Graph.Gateway.Options
{
    public class ServiceOptions
    {
        public ServiceConfig ContactService { get; set; }
    }

    public class ServiceConfig
    {
        public string ServiceName { get; set; }
        public string Url { get; set; }
    }
}
