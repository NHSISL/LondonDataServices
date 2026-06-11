using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class OptOutPipelineAliveHealthCheckServiceException : Xeption
    {
        public OptOutPipelineAliveHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
