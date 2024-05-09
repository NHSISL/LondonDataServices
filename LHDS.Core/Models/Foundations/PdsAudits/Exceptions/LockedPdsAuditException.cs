using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.PdsAudits.Exceptions
{
    public class LockedPdsAuditException : Xeption
    {
        public LockedPdsAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}