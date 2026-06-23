using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException : Xeption
    {
        public FailedIngestionTrackingDeletedWithoutCompletionHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
