using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class FailedResolvedAddressStorageException : Xeption
    {
        public FailedResolvedAddressStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}