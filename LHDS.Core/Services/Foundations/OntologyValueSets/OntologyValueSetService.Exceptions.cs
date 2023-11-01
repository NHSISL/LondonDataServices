using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.OntologyValueSets;
using LHDS.Core.Models.Foundations.OntologyValueSets.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyValueSets
{
    public partial class OntologyValueSetService
    {
        private delegate ValueTask<OntologyValueSet> ReturningOntologyValueSetFunction();
        private delegate IQueryable<OntologyValueSet> ReturningOntologyValueSetsFunction();

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
            catch (NotFoundOntologyValueSetException notFoundOntologyValueSetException)
            {
                throw CreateAndLogValidationException(notFoundOntologyValueSetException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOntologyValueSetException =
                    new AlreadyExistsOntologyValueSetException(
                        message: "OntologyValueSet with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsOntologyValueSetException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidOntologyValueSetReferenceException =
                    new InvalidOntologyValueSetReferenceException(
                        message: "Invalid ontologyValueSet reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidOntologyValueSetReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedOntologyValueSetException = 
                    new LockedOntologyValueSetException(
                        message: "Locked ontologyValueSet record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedOntologyValueSetException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOntologyValueSetStorageException =
                    new FailedOntologyValueSetStorageException(
                        message: "Failed ontologyValueSet storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedOntologyValueSetStorageException);
            }
            catch (Exception exception)
            {
                var failedOntologyValueSetServiceException =
                    new FailedOntologyValueSetServiceException(
                        message: "Failed ontologyValueSet service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyValueSetServiceException);
            }
        }

        private IQueryable<OntologyValueSet> TryCatch(ReturningOntologyValueSetsFunction returningOntologyValueSetsFunction)
        {
            try
            {
                return returningOntologyValueSetsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedOntologyValueSetStorageException =
                    new FailedOntologyValueSetStorageException(
                        message: "Failed ontologyValueSet storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyValueSetStorageException);
            }
            catch (Exception exception)
            {
                var failedOntologyValueSetServiceException =
                    new FailedOntologyValueSetServiceException(
                        message: "Failed ontologyValueSet service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyValueSetServiceException);
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

        private OntologyValueSetDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ontologyValueSetDependencyValidationException =
                new OntologyValueSetDependencyValidationException(
                    message: "OntologyValueSet dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyValueSetDependencyValidationException);

            return ontologyValueSetDependencyValidationException;
        }

        private OntologyValueSetDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ontologyValueSetDependencyException = 
                new OntologyValueSetDependencyException(
                    message: "OntologyValueSet dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(ontologyValueSetDependencyException);

            return ontologyValueSetDependencyException;
        }

        private OntologyValueSetServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ontologyValueSetServiceException = 
                new OntologyValueSetServiceException(
                    message: "OntologyValueSet service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyValueSetServiceException);

            return ontologyValueSetServiceException;
        }
    }
}