namespace CRM.Shared.CorrelationId
{
    public interface ICorrelationContextFactory
    {
        CorrelationContext Create(string correlationId, string header);
        void Dispose();
    }
}
