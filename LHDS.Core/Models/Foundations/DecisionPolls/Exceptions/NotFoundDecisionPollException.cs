// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.DecisionPolls.Exceptions
{
    public class NotFoundDecisionPollException : Xeption
    {
        public NotFoundDecisionPollException(Guid decisionPollId)
            : base(message: $"Couldn't find decisionPoll with decisionPollId: {decisionPollId}.")
        { }
    }
}