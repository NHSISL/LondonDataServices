using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class LockedAddressException : Xeption
    {
        public LockedAddressException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}