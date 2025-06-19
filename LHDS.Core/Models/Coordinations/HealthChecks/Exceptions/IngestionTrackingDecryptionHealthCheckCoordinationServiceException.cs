using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class IngestionTrackingDecryptionHealthCheckCooridinationServiceException : Xeption
    {
        public IngestionTrackingDecryptionHealthCheckCooridinationServiceException(
            string message, 
            Exception innerException
        )
            : base(message, innerException)
        { }
    }
}
