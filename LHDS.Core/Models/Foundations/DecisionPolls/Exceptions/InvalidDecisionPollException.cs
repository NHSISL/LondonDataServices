// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class InvalidDecisionPollException : Xeption
    {
        public InvalidDecisionPollException(string message)
            : base(message)
        { }
    }
}
