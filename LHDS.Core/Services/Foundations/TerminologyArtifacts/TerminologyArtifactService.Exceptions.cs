// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        private delegate ValueTask<IQueryable<TerminologyArtifact>> ReturningTerminologyArtifactsFunction();

        private async ValueTask<TerminologyArtifact> TryCatch(
            ReturningTerminologyArtifactFunction returningTerminologyArtifactFunction)
        {
            try
            {
                return await returningTerminologyArtifactFunction();
            }
            catch (NullTerminologyArtifactException nullTerminologyArtifactException)
            {
                throw await  CreateAndLogValidationExceptionAsync(nullTerminologyArtifactException);
            }
            catch (InvalidTerminologyArtifactException invalidTerminologyArtifactException)
            {
                throw await  CreateAndLogValidationExceptionAsync(invalidTerminologyArtifactException);
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedTerminologyArtifactStorageException);
            }
            catch (NotFoundTerminologyArtifactException notFoundTerminologyArtifactException)
            {
                throw await  CreateAndLogValidationExceptionAsync(notFoundTerminologyArtifactException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsTerminologyArtifactException =
                    new AlreadyExistsTerminologyArtifactException(
                        message: "TerminologyArtifact with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await  CreateAndLogDependencyValidationExceptionAsync(alreadyExistsTerminologyArtifactException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidTerminologyArtifactReferenceException =
                    new InvalidTerminologyArtifactReferenceException(
                        message: "Invalid terminologyArtifact reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await  CreateAndLogDependencyValidationExceptionAsync(invalidTerminologyArtifactReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedTerminologyArtifactException =
                    new LockedTerminologyArtifactException(
                        message: "Locked terminologyArtifact record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await  CreateAndLogDependencyValidationExceptionAsync(lockedTerminologyArtifactException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await  CreateAndLogDependencyExceptionAsync(failedTerminologyArtifactStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactServiceException =
                    new FailedTerminologyArtifactServiceException(
                        message: "Failed terminologyArtifact service error occurred, please contact support.",
                        innerException: exception);

                throw await  CreateAndLogServiceExceptionAsync(failedTerminologyArtifactServiceException);
            }
        }

        private async ValueTask<IQueryable<TerminologyArtifact>> TryCatch(
            ReturningTerminologyArtifactsFunction returningTerminologyArtifactsFunction)
        {
            try
            {
                return await returningTerminologyArtifactsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedTerminologyArtifactStorageException =
                    new FailedTerminologyArtifactStorageException(
                        message: "Failed terminologyArtifact storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await  CreateAndLogCriticalDependencyExceptionAsync(failedTerminologyArtifactStorageException);
            }
            catch (Exception exception)
            {
                var failedTerminologyArtifactServiceException =
                    new FailedTerminologyArtifactServiceException(
                        message: "Failed terminologyArtifact service error occurred, please contact support.",
                        innerException: exception);

                throw await  CreateAndLogServiceExceptionAsync(failedTerminologyArtifactServiceException);
            }
        }

        private async ValueTask<TerminologyArtifactValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var terminologyArtifactValidationException =
                new TerminologyArtifactValidationException(
                    message: "TerminologyArtifact validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactValidationException);

            return terminologyArtifactValidationException;
        }

        private async ValueTask<TerminologyArtifactDependencyException> CreateAndLogCriticalDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(terminologyArtifactDependencyException);

            return terminologyArtifactDependencyException;
        }

        private async ValueTask<TerminologyArtifactDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var terminologyArtifactDependencyValidationException =
                new TerminologyArtifactDependencyValidationException(
                    message: "TerminologyArtifact dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactDependencyValidationException);

            return terminologyArtifactDependencyValidationException;
        }

        private async ValueTask<TerminologyArtifactDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var terminologyArtifactDependencyException =
                new TerminologyArtifactDependencyException(
                    message: "TerminologyArtifact dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactDependencyException);

            return terminologyArtifactDependencyException;
        }

        private async ValueTask<TerminologyArtifactServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var terminologyArtifactServiceException =
                new TerminologyArtifactServiceException(
                    message: "TerminologyArtifact service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(terminologyArtifactServiceException);

            return terminologyArtifactServiceException;
        }
    }
}