using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressPipelineAliveHealthCheckServiceException : Xeption
    {
        public ResolvedAddressPipelineAliveHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
