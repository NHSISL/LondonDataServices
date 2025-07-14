using System;

namespace LHDS.Core.Models.Foundations.HealthChecks.ResolvedAddresses
{
    public class ResolvedAddressMatchGroup
    {
        public string CorrelationId { get; set; }
        public DateTimeOffset ProcessStartDateTime { get; set; }
    }
}
