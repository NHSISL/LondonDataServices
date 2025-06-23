using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedTerminologyPollsFailedToProcessHealthCheckServiceException : Xeption
    {
        public FailedTerminologyPollsFailedToProcessHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
