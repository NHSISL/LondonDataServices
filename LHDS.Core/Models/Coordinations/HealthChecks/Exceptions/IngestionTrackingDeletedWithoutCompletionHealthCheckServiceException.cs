using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingDeletedWithoutCompletionHealthCheckServiceException : Xeption
    {
        public IngestionTrackingDeletedWithoutCompletionHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
