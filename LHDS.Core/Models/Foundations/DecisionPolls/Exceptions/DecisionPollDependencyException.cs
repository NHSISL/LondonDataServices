// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class DecisionPollDependencyException : Xeption
    {
        public DecisionPollDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}