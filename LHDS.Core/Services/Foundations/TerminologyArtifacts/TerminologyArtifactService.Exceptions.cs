using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService
    {
        private delegate ValueTask<TerminologyArtifact> ReturningTerminologyArtifactFunction();

        private async ValueTask<TerminologyArtifact> TryCatch(ReturningTerminologyArtifactFunction returningTerminologyArtifactFunction)
        {
            try
            {
                return await returningTerminologyArtifactFunction();
            }
            catch (NullTerminologyArtifactException nullTerminologyArtifactException)
            {
                throw CreateAndLogValidationException(nullTerminologyArtifactException);
            }
        }

        private TerminologyArtifactValidationException CreateAndLogValidationException(Xeption exception)
        {
            var terminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactValidationException);

            return terminologyArtifactValidationException;
        }
    }
}