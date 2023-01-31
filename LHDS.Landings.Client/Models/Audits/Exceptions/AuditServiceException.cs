using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class AuditServiceException : Xeption
    {
        public AuditServiceException(Exception innerException)
            : base(message: "Audit service error occurred, contact support.", innerException)
        { }
    }
}