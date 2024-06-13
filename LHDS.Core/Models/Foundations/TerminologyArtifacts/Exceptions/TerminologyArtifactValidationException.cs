using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactValidationException : Xeption
    {
        public TerminologyArtifactValidationException(string message, Xeption? innerException)
            : base(message,innerException)
        { }
    }
}