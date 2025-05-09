// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.HealthChecks.Exceptions
{
    public class HealthCheckPublisherServiceException : Xeption
    {
        public HealthCheckPublisherServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}