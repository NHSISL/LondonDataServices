// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class TerminologyPollValidationException : Xeption
    {
        public TerminologyPollValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}