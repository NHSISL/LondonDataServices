using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class OptOutsExpiredOptOutsHealthCheckServiceException : Xeption
    {
        public OptOutsExpiredOptOutsHealthCheckServiceException(
            string message,
            Exception innerException) 
            : base(message, innerException)
        { }
    }
}
