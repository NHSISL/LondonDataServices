using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedPdsReceivedReplyHealthCheckServiceException : Xeption
    {
        public FailedPdsReceivedReplyHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
