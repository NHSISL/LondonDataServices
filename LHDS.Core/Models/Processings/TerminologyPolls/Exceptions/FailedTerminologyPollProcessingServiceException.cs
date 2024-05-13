// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class FailedTerminologyPollProcessingServiceException : Xeption
    {
        public FailedTerminologyPollProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
