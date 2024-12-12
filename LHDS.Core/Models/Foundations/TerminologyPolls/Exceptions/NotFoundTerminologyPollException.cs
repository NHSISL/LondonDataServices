// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class NotFoundTerminologyPollException : Xeption
    {
        public NotFoundTerminologyPollException(Guid terminologyPollId)
            : base(message: $"Couldn't find terminologyPoll with terminologyPollId: {terminologyPollId}.")
        { }
    }
}