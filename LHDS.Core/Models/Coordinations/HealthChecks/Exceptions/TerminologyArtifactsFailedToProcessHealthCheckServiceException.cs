using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class TerminologyArtifactsFailedToProcessHealthCheckServiceException : Xeption
    {
        public TerminologyArtifactsFailedToProcessHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
