// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.SubscriberPractices;
using LHDS.Core.Models.Foundations.SubscriberPractices.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberPractices
{
    public partial class SubscriberPracticeService
    {
        private delegate ValueTask<SubscriberPractice> ReturningSubscriberPracticeFunction();
        private delegate ValueTask<IQueryable<SubscriberPractice>> ReturningSubscriberPracticesFunction();

        private async ValueTask<SubscriberPractice> TryCatch(
            ReturningSubscriberPracticeFunction returningSubscriberPracticeFunction)
        {
            try
            {
                return await returningSubscriberPracticeFunction();
            }
            catch (NullSubscriberPracticeException nullSubscriberPracticeException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberPracticeException);
            }
            catch (InvalidSubscriberPracticeException invalidSubscriberPracticeException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSubscriberPracticeException);
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberPracticeStorageException =
                    new FailedSubscriberPracticeStorageException(
                        message: "Failed subscriberPractice storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSubscriberPracticeStorageException);
            }
            catch (NotFoundSubscriberPracticeException notFoundSubscriberPracticeException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundSubscriberPracticeException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSubscriberPracticeException =
                    new AlreadyExistsSubscriberPracticeException(
                        message: "SubscriberPractice with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsSubscriberPracticeException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSubscriberPracticeReferenceException =
                    new InvalidSubscriberPracticeReferenceException(
                        message: "Invalid subscriberPractice reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(
                    invalidSubscriberPracticeReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSubscriberPracticeException =
                    new LockedSubscriberPracticeException(
                        message: "Locked subscriberPractice record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedSubscriberPracticeException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSubscriberPracticeStorageException =
                    new FailedSubscriberPracticeStorageException(
                        message: "Failed subscriberPractice storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedSubscriberPracticeStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberPracticeServiceException =
                    new FailedSubscriberPracticeServiceException(
                        message: "Failed subscriberPractice service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberPracticeServiceException);
            }
        }

        private async ValueTask<IQueryable<SubscriberPractice>>
            TryCatch(ReturningSubscriberPracticesFunction returningSubscriberPracticesFunction)
        {
            try
            {
                return await returningSubscriberPracticesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberPracticeStorageException =
                    new FailedSubscriberPracticeStorageException(
                        message: "Failed subscriberPractice storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSubscriberPracticeStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberPracticeServiceException =
                    new FailedSubscriberPracticeServiceException(
                        message: "Failed subscriberPractice service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberPracticeServiceException);
            }
        }

        private async ValueTask<SubscriberPracticeValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var subscriberPracticeValidationException =
                new SubscriberPracticeValidationException(
                    message: "SubscriberPractice validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberPracticeValidationException);

            return subscriberPracticeValidationException;
        }

        private async ValueTask<SubscriberPracticeDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var subscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(subscriberPracticeDependencyException);

            return subscriberPracticeDependencyException;
        }

        private async ValueTask<SubscriberPracticeDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var subscriberPracticeDependencyValidationException =
                new SubscriberPracticeDependencyValidationException(
                    message: "SubscriberPractice dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberPracticeDependencyValidationException);

            return subscriberPracticeDependencyValidationException;
        }

        private async ValueTask<SubscriberPracticeDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var subscriberPracticeDependencyException =
                new SubscriberPracticeDependencyException(
                    message: "SubscriberPractice dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberPracticeDependencyException);

            return subscriberPracticeDependencyException;
        }

        private async ValueTask<SubscriberPracticeServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var subscriberPracticeServiceException =
                new SubscriberPracticeServiceException(
                    message: "SubscriberPractice service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberPracticeServiceException);

            return subscriberPracticeServiceException;
        }
    }
}