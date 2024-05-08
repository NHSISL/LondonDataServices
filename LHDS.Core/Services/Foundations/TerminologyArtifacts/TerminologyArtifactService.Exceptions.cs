// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.TerminologyArtifacts;
using LHDS.Core.Models.Foundations.TerminologyArtifacts.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.TerminologyArtifacts
{
    public partial class TerminologyArtifactService
    {
        private delegate ValueTask<TerminologyArtifact> ReturningTerminologyArtifactFunction();
        private delegate IQueryable<TerminologyArtifact> ReturningTerminologyArtifactsFunction();

        private async ValueTask<TerminologyArtifact> TryCatch(
            ReturningTerminologyArtifactFunction returningTerminologyArtifactFunction)
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
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyArtifactStorageException);
            }
            catch (NotFoundTerminologyArtifactException notFoundTerminologyArtifactException)
            {
                throw CreateAndLogValidationException(notFoundTerminologyArtifactException);
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTerminologyArtifactException =
                    new LockedTerminologyArtifactException(
                        message: "Locked terminologyArtifact record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedTerminologyArtifactException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedTerminologyArtifactStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactServiceException =
                    new FailedTerminologyArtifactServiceException(
                        message: "Failed terminologyArtifact service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyArtifactServiceException);
            }
        }

        private IQueryable<TerminologyArtifact> TryCatch(
            ReturningTerminologyArtifactsFunction returningTerminologyArtifactsFunction)
        {
            try
            {
                return returningTerminologyArtifactsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedTerminologyArtifactStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactServiceException =
                    new FailedTerminologyArtifactServiceException(
                        message: "Failed terminologyArtifact service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedTerminologyArtifactServiceException);
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
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(terminologyArtifactDependencyException);

            return terminologyArtifactDependencyException;
        }

        private TerminologyArtifactDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var terminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactDependencyValidationException);

            return terminologyArtifactDependencyValidationException;
        }

        private TerminologyArtifactDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var terminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactDependencyException);

            return terminologyArtifactDependencyException;
        }

        private TerminologyArtifactServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var terminologyArtifactServiceException =
                new TerminologyArtifactServiceException(
                    message: "TerminologyArtifact service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(terminologyArtifactServiceException);

            return terminologyArtifactServiceException;
        }
    }
}