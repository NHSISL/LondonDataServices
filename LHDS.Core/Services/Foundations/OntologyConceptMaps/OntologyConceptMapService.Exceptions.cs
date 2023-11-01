using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.OntologyConceptMaps;
using LHDS.Core.Models.Foundations.OntologyConceptMaps.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.OntologyConceptMaps
{
    public partial class OntologyConceptMapService
    {
        private delegate ValueTask<OntologyConceptMap> ReturningOntologyConceptMapFunction();
        private delegate IQueryable<OntologyConceptMap> ReturningOntologyConceptMapsFunction();

        private async ValueTask<OntologyConceptMap> TryCatch(ReturningOntologyConceptMapFunction returningOntologyConceptMapFunction)
        {
            try
            {
                return await returningOntologyConceptMapFunction();
            }
            catch (NullOntologyConceptMapException nullOntologyConceptMapException)
            {
                throw CreateAndLogValidationException(nullOntologyConceptMapException);
            }
            catch (InvalidOntologyConceptMapException invalidOntologyConceptMapException)
            {
                throw CreateAndLogValidationException(invalidOntologyConceptMapException);
            }
            catch (SqlException sqlException)
            {
                var failedOntologyConceptMapStorageException =
                    new FailedOntologyConceptMapStorageException(
                        message: "Failed ontologyConceptMap storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyConceptMapStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsOntologyConceptMapException =
                    new AlreadyExistsOntologyConceptMapException(
                        message: "OntologyConceptMap with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsOntologyConceptMapException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidOntologyConceptMapReferenceException =
                    new InvalidOntologyConceptMapReferenceException(
                        message: "Invalid ontologyConceptMap reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidOntologyConceptMapReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedOntologyConceptMapStorageException =
                    new FailedOntologyConceptMapStorageException(
                        message: "Failed ontologyConceptMap storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedOntologyConceptMapStorageException);
            }
            catch (Exception exception)
            {
                var failedOntologyConceptMapServiceException =
                    new FailedOntologyConceptMapServiceException(
                        message: "Failed ontologyConceptMap service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyConceptMapServiceException);
            }
        }

        private IQueryable<OntologyConceptMap> TryCatch(ReturningOntologyConceptMapsFunction returningOntologyConceptMapsFunction)
        {
            try
            {
                return returningOntologyConceptMapsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedOntologyConceptMapStorageException =
                    new FailedOntologyConceptMapStorageException(
                        message: "Failed ontologyConceptMap storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedOntologyConceptMapStorageException);
            }
            catch (Exception exception)
            {
                var failedOntologyConceptMapServiceException =
                    new FailedOntologyConceptMapServiceException(
                        message: "Failed ontologyConceptMap service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedOntologyConceptMapServiceException);
            }
        }

        private OntologyConceptMapValidationException CreateAndLogValidationException(Xeption exception)
        {
            var ontologyConceptMapValidationException =
                new OntologyConceptMapValidationException(
                    message: "OntologyConceptMap validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapValidationException);

            return ontologyConceptMapValidationException;
        }

        private OntologyConceptMapDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var ontologyConceptMapDependencyException = 
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(ontologyConceptMapDependencyException);

            return ontologyConceptMapDependencyException;
        }

        private OntologyConceptMapDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var ontologyConceptMapDependencyValidationException =
                new OntologyConceptMapDependencyValidationException(
                    message: "OntologyConceptMap dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapDependencyValidationException);

            return ontologyConceptMapDependencyValidationException;
        }

        private OntologyConceptMapDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var ontologyConceptMapDependencyException = 
                new OntologyConceptMapDependencyException(
                    message: "OntologyConceptMap dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapDependencyException);

            return ontologyConceptMapDependencyException;
        }

        private OntologyConceptMapServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var ontologyConceptMapServiceException = 
                new OntologyConceptMapServiceException(
                    message: "OntologyConceptMap service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(ontologyConceptMapServiceException);

            return ontologyConceptMapServiceException;
        }
    }
}