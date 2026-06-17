using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedOptOutPipelineAliveHealthCheckServiceException : Xeption
    {
        public FailedOptOutPipelineAliveHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
