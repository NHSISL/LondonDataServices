using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class PdsReceivedReplyHealthCheckServiceException : Xeption
    {
        public PdsReceivedReplyHealthCheckServiceException(
            string message,
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
