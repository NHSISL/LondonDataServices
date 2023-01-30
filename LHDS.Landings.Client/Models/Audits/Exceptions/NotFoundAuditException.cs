using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class NotFoundAuditException : Xeption
    {
        public NotFoundAuditException(Guid auditId)
            : base(message: $"Couldn't find audit with auditId: {auditId}.")
        { }
    }
}