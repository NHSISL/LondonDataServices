// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decisions.Exceptions
{
    public class FailedDecisionCoordinationServiceException : Xeption
    {
        public FailedDecisionCoordinationServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}