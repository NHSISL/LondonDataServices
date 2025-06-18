using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingFilesReceivedHealthCheckServiceException : Xeption
    {
        public IngestionTrackingFilesReceivedHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}