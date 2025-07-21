using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedResolvedAddressFailedToProcessHealthCheckServiceException : Xeption
    {
        public FailedResolvedAddressFailedToProcessHealthCheckServiceException(
            string message, 
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
