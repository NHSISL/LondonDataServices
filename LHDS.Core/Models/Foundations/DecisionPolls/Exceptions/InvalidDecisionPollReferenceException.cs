// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class InvalidDecisionPollReferenceException : Xeption
    {
        public InvalidDecisionPollReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}