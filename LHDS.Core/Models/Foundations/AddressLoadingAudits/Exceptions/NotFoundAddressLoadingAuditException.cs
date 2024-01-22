using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class NotFoundAddressLoadingAuditException : Xeption
    {
        public NotFoundAddressLoadingAuditException(Guid addressLoadingAuditId)
            : base(message: $"Couldn't find addressLoadingAudit with addressLoadingAuditId: {addressLoadingAuditId}.")
        { }
    }
}