// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class LockedDecisionPollException : Xeption
    {
        public LockedDecisionPollException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}