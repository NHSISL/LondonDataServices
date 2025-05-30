using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingFailedToProcessHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingFailedToProcessHealthCheckServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
