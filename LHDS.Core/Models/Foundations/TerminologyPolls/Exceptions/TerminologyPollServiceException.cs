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