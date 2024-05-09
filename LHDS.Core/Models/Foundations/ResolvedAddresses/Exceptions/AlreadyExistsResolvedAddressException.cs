using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class AlreadyExistsResolvedAddressException : Xeption
    {
        public AlreadyExistsResolvedAddressException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}