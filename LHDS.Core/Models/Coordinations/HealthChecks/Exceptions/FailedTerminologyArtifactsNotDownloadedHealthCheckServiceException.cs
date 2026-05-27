using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedTerminologyArtifactsNotDownloadedHealthCheckServiceException : Xeption
    {
        public FailedTerminologyArtifactsNotDownloadedHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
