// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class TerminologyPollProcessingServiceException : Xeption
    {
        public TerminologyPollProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
