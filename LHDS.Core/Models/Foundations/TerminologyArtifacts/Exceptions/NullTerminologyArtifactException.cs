using Xeptions;

namespace LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions
{
    public class NullTerminologyArtifactException : Xeption
    {
        public NullTerminologyArtifactException(string message)
            : base(message)
        { }
    }
}