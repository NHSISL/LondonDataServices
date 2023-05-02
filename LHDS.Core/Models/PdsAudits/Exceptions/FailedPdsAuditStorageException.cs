using System;
using Xeptions;

namespace LHDS.Core.Models.PdsAudits.Exceptions
{
    public class FailedPdsAuditStorageException : Xeption
    {
        public FailedPdsAuditStorageException(Exception innerException)
            : base(message: "Failed pdsAudit storage error occurred, contact support.", innerException)
        { }
    }
}