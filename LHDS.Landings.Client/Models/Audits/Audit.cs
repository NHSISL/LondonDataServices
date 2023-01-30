using System;

namespace LHDS.Landings.Client.Models.Audits
{
    public class Audit
    {
        public Guid Id { get; set; }
        public string IngestionTrackingId { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
