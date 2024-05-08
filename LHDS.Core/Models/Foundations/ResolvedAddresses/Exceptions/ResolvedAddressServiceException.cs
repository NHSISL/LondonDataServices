using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressServiceException : Xeption
    {
        public ResolvedAddressServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}