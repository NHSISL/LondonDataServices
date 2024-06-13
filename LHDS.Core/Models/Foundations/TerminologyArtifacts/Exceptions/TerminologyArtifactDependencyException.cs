using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class TerminologyArtifactDependencyException : Xeption
    {
        public TerminologyArtifactDependencyException(string message, Xeption? innerException) 
            : base(message, innerException)
        { }
    }
}