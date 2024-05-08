using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class AuditServiceException : Xeption
    {
        public AuditServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}