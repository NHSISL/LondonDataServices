using System;
using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class AlreadyExistsPdsAuditException : Xeption
    {
        public AlreadyExistsPdsAuditException(Exception innerException)
            : base(message: "PdsAudit with the same Id already exists.", innerException)
        { }
    }
}