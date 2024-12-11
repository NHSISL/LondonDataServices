// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class TerminologyPollProcessingDependencyException : Xeption
    {
        public TerminologyPollProcessingDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
