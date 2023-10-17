using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class LockedAddressExtractionAuditException : Xeption
    {
        public LockedAddressExtractionAuditException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}