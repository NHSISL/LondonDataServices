using System.Threading.Tasks;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemService
    {
        private delegate ValueTask<OntologyCodeSystem> ReturningOntologyCodeSystemFunction();

        private async ValueTask<OntologyCodeSystem> TryCatch(ReturningOntologyCodeSystemFunction returningOntologyCodeSystemFunction)
        {
            try
            {
                return await returningOntologyCodeSystemFunction();
            }
            catch (NullOntologyCodeSystemException nullOntologyCodeSystemException)
            {
                throw CreateAndLogValidationException(nullOntologyCodeSystemException);
            }
            catch (InvalidOntologyCodeSystemException invalidOntologyCodeSystemException)
            {
                throw CreateAndLogValidationException(invalidOntologyCodeSystemException);
            }
        }

        private OntologyCodeSystemValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyCodeSystemValidationException =
                new OntologyCodeSystemValidationException(
                    message: "OntologyCodeSystem validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyCodeSystemValidationException);

            return ontologyCodeSystemValidationException;
        }
    }
}