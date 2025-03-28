// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private delegate ValueTask<SubscriberAgreement> ReturningSubscriberAgreementFunction();
        private delegate ValueTask<IQueryable<SubscriberAgreement>> ReturningSubscriberAgreementsFunction();

        private async ValueTask<SubscriberAgreement> TryCatch(
            ReturningSubscriberAgreementFunction returningSubscriberAgreementFunction)
        {
            try
            {
                return await returningSubscriberAgreementFunction();
            }
            catch (NullSubscriberAgreementException nullSubscriberAgreementException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSubscriberAgreementException);
            }
            catch (InvalidSubscriberAgreementException invalidSubscriberAgreementException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSubscriberAgreementException);
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSubscriberAgreementStorageException);
            }
            catch (NotFoundSubscriberAgreementException notFoundSubscriberAgreementException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundSubscriberAgreementException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSubscriberAgreementException =
                    new AlreadyExistsSubscriberAgreementException(
                        message: "SubscriberAgreement with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsSubscriberAgreementException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSubscriberAgreementReferenceException =
                    new InvalidSubscriberAgreementReferenceException(
                        message: "Invalid subscriberAgreement reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(
                    invalidSubscriberAgreementReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSubscriberAgreementException =
                    new LockedSubscriberAgreementException(
                        message: "Locked subscriberAgreement record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedSubscriberAgreementException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedSubscriberAgreementStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementServiceException =
                    new FailedSubscriberAgreementServiceException(
                        message: "Failed subscriberAgreement service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberAgreementServiceException);
            }
        }

        private async ValueTask<IQueryable<SubscriberAgreement>>
            TryCatch(ReturningSubscriberAgreementsFunction returningSubscriberAgreementsFunction)
        {
            try
            {
                return await returningSubscriberAgreementsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSubscriberAgreementStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementServiceException =
                    new FailedSubscriberAgreementServiceException(
                        message: "Failed subscriberAgreement service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSubscriberAgreementServiceException);
            }
        }

        private async ValueTask<SubscriberAgreementValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var subscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementValidationException);

            return subscriberAgreementValidationException;
        }

        private async ValueTask<SubscriberAgreementDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var subscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(subscriberAgreementDependencyException);

            return subscriberAgreementDependencyException;
        }

        private async ValueTask<SubscriberAgreementDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var subscriberAgreementDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: "SubscriberAgreement dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementDependencyValidationException);

            return subscriberAgreementDependencyValidationException;
        }

        private async ValueTask<SubscriberAgreementDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var subscriberAgreementDependencyException =
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementDependencyException);

            return subscriberAgreementDependencyException;
        }

        private async ValueTask<SubscriberAgreementServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var subscriberAgreementServiceException =
                new SubscriberAgreementServiceException(
                    message: "SubscriberAgreement service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(subscriberAgreementServiceException);

            return subscriberAgreementServiceException;
        }
    }
}