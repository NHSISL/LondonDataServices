using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class InvalidTerminologyPollReferenceException : Xeption
    {
        public InvalidTerminologyPollReferenceException(string message, Exception? innerException)
            : base(message, innerException) { }
    }
}