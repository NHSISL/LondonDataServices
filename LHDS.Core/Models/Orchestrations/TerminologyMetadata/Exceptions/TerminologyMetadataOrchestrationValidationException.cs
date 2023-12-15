using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMedata.Exceptions
{
    public class TerminologyMetadataOrchestrationValidationException : Xeption
    {
        public TerminologyMetadataOrchestrationValidationException(string message, Xeption innerException)
            : base(message,innerException)
        { }
    }
}