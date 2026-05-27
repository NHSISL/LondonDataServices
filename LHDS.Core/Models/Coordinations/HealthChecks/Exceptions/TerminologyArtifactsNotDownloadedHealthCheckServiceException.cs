using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class TerminologyArtifactsNotDownloadedHealthCheckServiceException : Xeption
    {
        public TerminologyArtifactsNotDownloadedHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
