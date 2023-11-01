using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.OntologyCodeSystems;
using LHDS.Core.Models.Foundations.OntologyCodeSystems.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyCodeSystems
{
    public partial class OntologyCodeSystemService
    {
        private delegate ValueTask<OntologyCodeSystem> ReturningOntologyCodeSystemFunction();
        private delegate IQueryable<OntologyCodeSystem> ReturningOntologyCodeSystemsFunction();

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
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOntologyCodeSystemStorageException =
                    new FailedOntologyCodeSystemStorageException(
                        message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedOntologyCodeSystemStorageException);
            }
            catch (Exception exception)
            {
                var failedOntologyCodeSystemServiceException =
                    new FailedOntologyCodeSystemServiceException(
                        message: "Failed ontologyCodeSystem service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyCodeSystemServiceException);
            }
        }

        private IQueryable<OntologyCodeSystem> TryCatch(ReturningOntologyCodeSystemsFunction returningOntologyCodeSystemsFunction)
        {
            try
            {
                return returningOntologyCodeSystemsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedOntologyCodeSystemStorageException =
                    new FailedOntologyCodeSystemStorageException(
                        message: "Failed ontologyCodeSystem storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyCodeSystemStorageException);
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

        private OntologyCodeSystemDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ontologyCodeSystemDependencyException = 
                new OntologyCodeSystemDependencyException(
                    message: "OntologyCodeSystem dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyCodeSystemDependencyException);

            return ontologyCodeSystemDependencyException;
        }

        private OntologyCodeSystemServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ontologyCodeSystemServiceException = 
                new OntologyCodeSystemServiceException(
                    message: "OntologyCodeSystem service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyCodeSystemServiceException);

            return ontologyCodeSystemServiceException;
        }
    }
}