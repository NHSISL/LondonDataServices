using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class ResolvedAddressFailedToExportHealthCheckServiceException : Xeption
    {
        public ResolvedAddressFailedToExportHealthCheckServiceException(
            string message, 
            Exception innerException)
            : base(message, innerException)
        { }
    }
}
