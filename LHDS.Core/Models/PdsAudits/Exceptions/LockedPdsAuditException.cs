using System;
using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class LockedPdsAuditException : Xeption
    {
        public LockedPdsAuditException(Exception innerException)
            : base(message: "Locked pdsAudit record exception, please try again later", innerException)
        {
        }
    }
}