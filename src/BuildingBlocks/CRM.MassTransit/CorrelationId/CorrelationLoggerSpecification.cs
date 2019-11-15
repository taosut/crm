using System.Collections.Generic;
using System.Linq;
using GreenPipes;
using MassTransit;

namespace CRM.MassTransit.CorrelationId
{
    public class CorrelationLoggerSpecification : IPipeSpecification<ConsumeContext>
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            builder.AddFilter(new CorrelationIdLoggerFilter());
        }
    }
}
