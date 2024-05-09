using Xeptions;

namespace LHDS.Core.Models.Processings.TerminologyPolls.Exceptions
{
    public class TerminologyPollProcessingValidationException : Xeption
    {
        public TerminologyPollProcessingValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}