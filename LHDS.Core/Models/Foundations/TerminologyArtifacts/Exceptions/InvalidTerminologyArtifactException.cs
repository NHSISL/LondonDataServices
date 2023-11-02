using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class InvalidTerminologyArtifactException : Xeption
    {
        public InvalidTerminologyArtifactException(string message)
            : base(message)
        { }
    }
}