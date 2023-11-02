using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService
    {
        private void ValidateTerminologyArtifactOnAdd(TerminologyArtifact terminologyArtifact)
        {
            ValidateTerminologyArtifactIsNotNull(terminologyArtifact);
        }

        private static void ValidateTerminologyArtifactIsNotNull(TerminologyArtifact terminologyArtifact)
        {
            if (terminologyArtifact is null)
            {
                throw new NullTerminologyArtifactException(message: "TerminologyArtifact is null.");
            }
        }
    }
}