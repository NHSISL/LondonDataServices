using Xeptions;

namespace LHDS.Core.Models.Orchestrations.TerminologyMedata.Exceptions
{
    public class TerminologyMetadataOrchestrationResourceTypeException : Xeption
    {
        public TerminologyMetadataOrchestrationResourceTypeException(string message)
            : base(message)
        { }
    }
}