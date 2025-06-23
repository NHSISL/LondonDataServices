using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedResolvedAddressFailedToExportHealthCheckServiceException : Xeption
    {
        public FailedResolvedAddressFailedToExportHealthCheckServiceException(
            string message, 
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
