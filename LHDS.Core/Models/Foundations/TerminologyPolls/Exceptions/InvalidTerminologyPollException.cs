// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class InvalidTerminologyPollException : Xeption
    {
        public InvalidTerminologyPollException(string message)
            : base(message)
        { }
    }
}