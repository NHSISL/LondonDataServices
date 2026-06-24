using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingRetryWarningHealthCheckServiceException : Xeption
    {
        public IngestionTrackingRetryWarningHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
