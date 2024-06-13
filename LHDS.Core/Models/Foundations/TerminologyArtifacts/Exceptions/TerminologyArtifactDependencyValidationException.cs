using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactDependencyValidationException : Xeption
    {
        public TerminologyArtifactDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}