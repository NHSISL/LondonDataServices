using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class FailedAddressExtractionAuditServiceException : Xeption
    {
        public FailedAddressExtractionAuditServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}