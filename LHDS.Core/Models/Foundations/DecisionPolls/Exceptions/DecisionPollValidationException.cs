// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class DecisionPollValidationException : Xeption
    {
        public DecisionPollValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
