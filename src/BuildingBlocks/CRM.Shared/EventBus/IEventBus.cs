namespace CRM.Shared.EventBus
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);
        void Subscribe<T, TH>(string queueName)
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;
    }
}