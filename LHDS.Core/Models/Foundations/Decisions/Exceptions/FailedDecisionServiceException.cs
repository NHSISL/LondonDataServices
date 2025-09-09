// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Decisions.Exceptions
{
    public class FailedDecisionServiceException : Xeption
    {
        public FailedDecisionServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
