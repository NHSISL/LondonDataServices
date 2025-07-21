using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressFailedToProcessHealthCheckServiceException : Xeption
    {
        public ResolvedAddressFailedToProcessHealthCheckServiceException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
