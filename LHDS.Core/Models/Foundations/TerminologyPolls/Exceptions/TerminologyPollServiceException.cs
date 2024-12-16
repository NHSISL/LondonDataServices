// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class TerminologyPollServiceException : Xeption
    {
        public TerminologyPollServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}