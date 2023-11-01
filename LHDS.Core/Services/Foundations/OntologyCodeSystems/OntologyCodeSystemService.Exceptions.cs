using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (SqlException sqlException)
            {
                var failedOntologyCodeSystemStorageException =
                    new FailedOntologyCodeSystemStorageException(
                        message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyCodeSystemStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOntologyCodeSystemException =
                    new AlreadyExistsOntologyCodeSystemException(
                        message: "OntologyCodeSystem with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsOntologyCodeSystemException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidOntologyCodeSystemReferenceException =
                    new InvalidOntologyCodeSystemReferenceException(
                        message: "Invalid ontologyCodeSystem reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidOntologyCodeSystemReferenceException);
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

        private OntologyCodeSystemDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ontologyCodeSystemDependencyException = 
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(ontologyCodeSystemDependencyException);

            return ontologyCodeSystemDependencyException;
        }

        private OntologyCodeSystemDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ontologyCodeSystemDependencyValidationException =
                new OntologyCodeSystemDependencyValidationException(
                    message: "OntologyCodeSystem dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyCodeSystemDependencyValidationException);

            return ontologyCodeSystemDependencyValidationException;
        }
    }
}