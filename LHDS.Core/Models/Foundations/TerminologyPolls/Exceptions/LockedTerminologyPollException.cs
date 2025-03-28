// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class LockedTerminologyPollException : Xeption
    {
        public LockedTerminologyPollException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}