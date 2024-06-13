using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class AlreadyExistsTerminologyPollException : Xeption
    {
        public AlreadyExistsTerminologyPollException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}