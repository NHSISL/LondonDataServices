using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingRetryWarningHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingRetryWarningHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
