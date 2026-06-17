using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingDownloadHealthCheckServiceException : Xeption
    {
        public IngestionTrackingDownloadHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
