// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.HealthChecks.Exceptions
{
    public class FailedHealthCheckPublisherCoordinationServiceException : Xeption
    {
        public FailedHealthCheckPublisherCoordinationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}