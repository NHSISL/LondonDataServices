using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingDownloadHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingDownloadHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
