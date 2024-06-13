using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyPolls.Exceptions
{
    public class TerminologyPollDependencyValidationException : Xeption
    {
        public TerminologyPollDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}