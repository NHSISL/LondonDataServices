using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedIngestionTrackingDecryptionHealthCheckCooridinationServiceException : Xeption
    {
        public FailedIngestionTrackingDecryptionHealthCheckCooridinationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
