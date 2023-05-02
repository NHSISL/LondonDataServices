using System;
using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class FailedPdsAuditServiceException : Xeption
    {
        public FailedPdsAuditServiceException(Exception innerException)
            : base(message: "Failed pdsAudit service occurred, please contact support", innerException)
        { }
    }
}