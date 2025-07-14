using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressMatchQualityHealthCheckServiceException : Xeption
    {
        public ResolvedAddressMatchQualityHealthCheckServiceException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
