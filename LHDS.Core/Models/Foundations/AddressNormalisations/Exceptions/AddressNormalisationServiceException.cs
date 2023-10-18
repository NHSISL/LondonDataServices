using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class AddressNormalisationServiceException : Xeption
    {
        public AddressNormalisationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}