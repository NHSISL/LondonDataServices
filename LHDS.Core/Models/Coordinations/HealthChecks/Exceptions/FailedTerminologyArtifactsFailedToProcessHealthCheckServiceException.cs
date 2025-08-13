using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedTerminologyArtifactsFailedToProcessHealthCheckServiceException : Xeption
    {
        public FailedTerminologyArtifactsFailedToProcessHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
