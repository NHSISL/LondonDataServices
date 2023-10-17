using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions
{
    public class FailedAddressLoadingAuditStorageException : Xeption
    {
        public FailedAddressLoadingAuditStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}