using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class TerminologyPollDependencyException : Xeption
    {
        public TerminologyPollDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}