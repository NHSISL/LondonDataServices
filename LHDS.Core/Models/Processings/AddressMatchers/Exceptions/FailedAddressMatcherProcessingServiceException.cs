using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class FailedAddressMatcherProcessingServiceException : Xeption
    {
        public FailedAddressMatcherProcessingServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}