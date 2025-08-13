using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class TerminologyPollsNotPollingHealthCheckServiceException : Xeption
    {
        public TerminologyPollsNotPollingHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
