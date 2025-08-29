// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class NullDecisionPollException : Xeption
    {
        public NullDecisionPollException(string message)
            : base(message)
        { }
    }
}
