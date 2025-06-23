using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class TerminologyPollsFailedToProcessHealthCheckServiceException : Xeption
    {
        public TerminologyPollsFailedToProcessHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
