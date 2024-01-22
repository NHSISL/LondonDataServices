// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class InvalidArgumentTerminologyPollsProcessingException : Xeption
    {
        public InvalidArgumentTerminologyPollsProcessingException(string message)
            : base(message)
        { }
    }
}
