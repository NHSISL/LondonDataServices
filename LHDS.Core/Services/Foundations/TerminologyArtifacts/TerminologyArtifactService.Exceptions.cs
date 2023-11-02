using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
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
            catch (InvalidTerminologyArtifactException invalidTerminologyArtifactException)
            {
                throw CreateAndLogValidationException(invalidTerminologyArtifactException);
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyArtifactStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTerminologyArtifactException =
                    new AlreadyExistsTerminologyArtifactException(
                        message: "TerminologyArtifact with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsTerminologyArtifactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTerminologyArtifactReferenceException =
                    new InvalidTerminologyArtifactReferenceException(
                        message: "Invalid terminologyArtifact reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidTerminologyArtifactReferenceException);
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

        private TerminologyArtifactDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var terminologyArtifactDependencyException = 
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(terminologyArtifactDependencyException);

            return terminologyArtifactDependencyException;
        }

        private TerminologyArtifactDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var terminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactDependencyValidationException);

            return terminologyArtifactDependencyValidationException;
        }
    }
}