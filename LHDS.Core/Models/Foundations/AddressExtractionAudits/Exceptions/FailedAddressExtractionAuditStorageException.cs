using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions
{
    public class FailedAddressExtractionAuditStorageException : Xeption
    {
        public FailedAddressExtractionAuditStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}