using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditServiceException : Xeption
    {
        public PdsAuditServiceException(Exception innerException)
            : base(message: "PdsAudit service error occurred, contact support.", innerException)
        { }
    }
}