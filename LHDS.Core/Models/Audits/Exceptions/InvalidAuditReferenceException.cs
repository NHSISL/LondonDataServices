using System;
using Xeptions;

namespace LHDS.Core.Models.Audits.Exceptions
{
    public class InvalidAuditReferenceException : Xeption
    {
        public InvalidAuditReferenceException(Exception innerException)
            : base(message: "Invalid audit reference error occurred.", innerException) { }
    }
}