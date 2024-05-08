using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class AlreadyExistsAddressException : Xeption
    {
        public AlreadyExistsAddressException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}