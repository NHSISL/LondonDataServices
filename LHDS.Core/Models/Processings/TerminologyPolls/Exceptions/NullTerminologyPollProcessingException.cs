// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class NullTerminologyPollProcessingException : Xeption
    {
        public NullTerminologyPollProcessingException(string message)
            : base(message)
        { }
    }
}
