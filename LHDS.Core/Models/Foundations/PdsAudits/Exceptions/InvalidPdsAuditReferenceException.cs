using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class InvalidPdsAuditReferenceException : Xeption
    {
        public InvalidPdsAuditReferenceException(Exception innerException)
            : base(message: "Invalid pdsAudit reference error occurred.", innerException) { }
    }
}