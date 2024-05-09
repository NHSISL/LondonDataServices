using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Audits.Exceptions
{
    public class LockedAuditException : Xeption
    {
        public LockedAuditException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}