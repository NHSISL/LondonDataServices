using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressExportHealthCheckServiceException : Xeption
    {
        public ResolvedAddressExportHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
