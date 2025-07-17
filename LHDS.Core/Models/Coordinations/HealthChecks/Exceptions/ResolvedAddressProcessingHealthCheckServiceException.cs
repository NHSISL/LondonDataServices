using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressProcessingHealthCheckServiceException : Xeption
    {
        public ResolvedAddressProcessingHealthCheckServiceException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
