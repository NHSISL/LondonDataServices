using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingFailedToProcessHealthCheckServiceException : Xeption
    {
        public IngestionTrackingFailedToProcessHealthCheckServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
