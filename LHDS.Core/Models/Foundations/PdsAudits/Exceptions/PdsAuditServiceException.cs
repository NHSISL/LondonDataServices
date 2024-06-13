using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class PdsAuditServiceException : Xeption
    {
        public PdsAuditServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}