using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedResolvedAddressMatchQualityHealthCheckServiceException : Xeption
    {
        public FailedResolvedAddressMatchQualityHealthCheckServiceException(
            string message, 
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
