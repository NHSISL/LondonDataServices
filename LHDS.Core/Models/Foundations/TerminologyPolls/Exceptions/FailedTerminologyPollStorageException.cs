using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class FailedTerminologyPollStorageException : Xeption
    {
        public FailedTerminologyPollStorageException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}