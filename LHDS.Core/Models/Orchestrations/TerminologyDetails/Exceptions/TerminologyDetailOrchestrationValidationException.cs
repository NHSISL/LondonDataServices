using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyDetails.Exceptions
{
    public class TerminologyDetailOrchestrationValidationException : Xeption
    {
        public TerminologyDetailOrchestrationValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}