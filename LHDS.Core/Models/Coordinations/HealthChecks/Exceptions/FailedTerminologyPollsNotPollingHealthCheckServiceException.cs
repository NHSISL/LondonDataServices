using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedTerminologyPollsNotPollingHealthCheckServiceException : Xeption
    {
        public FailedTerminologyPollsNotPollingHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
