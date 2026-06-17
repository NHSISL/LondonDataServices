using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedResolvedAddressRetryWarningHealthCheckServiceException : Xeption
    {
        public FailedResolvedAddressRetryWarningHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
