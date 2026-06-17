using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingLastSeenHealthCheckServiceException : Xeption
    {
        public IngestionTrackingLastSeenHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
