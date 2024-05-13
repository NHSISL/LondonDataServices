using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class FailedResolvedAddressServiceException : Xeption
    {
        public FailedResolvedAddressServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}