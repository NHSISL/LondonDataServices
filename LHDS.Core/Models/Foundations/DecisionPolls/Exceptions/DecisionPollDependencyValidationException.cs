// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class DecisionPollDependencyValidationException : Xeption
    {
        public DecisionPollDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}