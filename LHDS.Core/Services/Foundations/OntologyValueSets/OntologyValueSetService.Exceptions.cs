using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetService
    {
        private delegate ValueTask<OntologyValueSet> ReturningOntologyValueSetFunction();

        private async ValueTask<OntologyValueSet> TryCatch(ReturningOntologyValueSetFunction returningOntologyValueSetFunction)
        {
            try
            {
                return await returningOntologyValueSetFunction();
            }
            catch (NullOntologyValueSetException nullOntologyValueSetException)
            {
                throw CreateAndLogValidationException(nullOntologyValueSetException);
            }
            catch (InvalidOntologyValueSetException invalidOntologyValueSetException)
            {
                throw CreateAndLogValidationException(invalidOntologyValueSetException);
            }
            catch (SqlException sqlException)
            {
                var failedOntologyValueSetStorageException =
                    new FailedOntologyValueSetStorageException(
                        message: "Failed ontologyValueSet storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyValueSetStorageException);
            }
        }

        private OntologyValueSetValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyValueSetValidationException =
                new OntologyValueSetValidationException(
                    message: "OntologyValueSet validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyValueSetValidationException);

            return ontologyValueSetValidationException;
        }

        private OntologyValueSetDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ontologyValueSetDependencyException = 
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(ontologyValueSetDependencyException);

            return ontologyValueSetDependencyException;
        }
    }
}