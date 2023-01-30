using System;
using Xeptions;

namespace LHDS.Landings.Client.Models.Audits.Exceptions
{
    public class FailedAuditServiceException : Xeption
    {
        public FailedAuditServiceException(Exception innerException)
            : base(message: "Failed audit service occurred, please contact support", innerException)
        { }
    }
}