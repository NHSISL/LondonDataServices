using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingIncompleteBatchHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingIncompleteBatchHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}