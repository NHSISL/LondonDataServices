using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedResolvedAddressMatchingProcessHealthCheckServiceException : Xeption
    {
        public FailedResolvedAddressMatchingProcessHealthCheckServiceException(
            string message, 
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
