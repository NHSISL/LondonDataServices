using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class FailedAddressNormalisationServiceException : Xeption
    {
        public FailedAddressNormalisationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}