using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class InvalidResolvedAddressReferenceException : Xeption
    {
        public InvalidResolvedAddressReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}