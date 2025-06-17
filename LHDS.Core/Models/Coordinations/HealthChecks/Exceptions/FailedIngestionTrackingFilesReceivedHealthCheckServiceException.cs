using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingFilesReceivedHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingFilesReceivedHealthCheckServiceException(
            string message,
            Exception innerException
            ) : base(message, innerException)
        { }
    }
}