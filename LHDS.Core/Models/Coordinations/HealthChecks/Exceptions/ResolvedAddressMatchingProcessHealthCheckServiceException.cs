using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressMatchingProcessHealthCheckServiceException : Xeption
    {
        public ResolvedAddressMatchingProcessHealthCheckServiceException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
