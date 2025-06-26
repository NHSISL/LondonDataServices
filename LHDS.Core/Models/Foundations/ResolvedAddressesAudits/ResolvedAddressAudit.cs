using System;
using LHDS.Core.Models.Bases;

namespace LHDS.Core.Models.Foundations.ResolvedAddressesAudits
{
    public class ResolvedAddressAudit : IKey, IAudit
    {
        public Guid Id { get; set; }
        public Guid UniqueResolvedAddressReference { get; set; }
        public Guid CorrelationId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
