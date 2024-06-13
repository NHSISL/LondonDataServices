using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private delegate ValueTask<SubscriberAgreement> ReturningSubscriberAgreementFunction();
        private delegate IQueryable<SubscriberAgreement> ReturningSubscriberAgreementsFunction();

        private async ValueTask<SubscriberAgreement> TryCatch(ReturningSubscriberAgreementFunction returningSubscriberAgreementFunction)
        {
            try
            {
                return await returningSubscriberAgreementFunction();
            }
            catch (NullSubscriberAgreementException nullSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(nullSubscriberAgreementException);
            }
            catch (InvalidSubscriberAgreementException invalidSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(invalidSubscriberAgreementException);
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSubscriberAgreementStorageException);
            }
            catch (NotFoundSubscriberAgreementException notFoundSubscriberAgreementException)
            {
                throw CreateAndLogValidationException(notFoundSubscriberAgreementException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSubscriberAgreementException =
                    new AlreadyExistsSubscriberAgreementException(
                        message: "SubscriberAgreement with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSubscriberAgreementException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSubscriberAgreementReferenceException =
                    new InvalidSubscriberAgreementReferenceException(
                        message: "Invalid subscriberAgreement reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidSubscriberAgreementReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSubscriberAgreementException = 
                    new LockedSubscriberAgreementException(
                        message: "Locked subscriberAgreement record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedSubscriberAgreementException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedSubscriberAgreementStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementServiceException =
                    new FailedSubscriberAgreementServiceException(
                        message: "Failed subscriberAgreement service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberAgreementServiceException);
            }
        }

        private IQueryable<SubscriberAgreement> TryCatch(ReturningSubscriberAgreementsFunction returningSubscriberAgreementsFunction)
        {
            try
            {
                return returningSubscriberAgreementsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSubscriberAgreementStorageException =
                    new FailedSubscriberAgreementStorageException(
                        message: "Failed subscriberAgreement storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSubscriberAgreementStorageException);
            }
            catch (Exception exception)
            {
                var failedSubscriberAgreementServiceException =
                    new FailedSubscriberAgreementServiceException(
                        message: "Failed subscriberAgreement service error occurred, please contact support.", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedSubscriberAgreementServiceException);
            }
        }

        private SubscriberAgreementValidationException CreateAndLogValidationException(Xeption exception)
        {
            var subscriberAgreementValidationException =
                new SubscriberAgreementValidationException(
                    message: "SubscriberAgreement validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberAgreementValidationException);

            return subscriberAgreementValidationException;
        }

        private SubscriberAgreementDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var subscriberAgreementDependencyException = 
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(subscriberAgreementDependencyException);

            return subscriberAgreementDependencyException;
        }

        private SubscriberAgreementDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var subscriberAgreementDependencyValidationException =
                new SubscriberAgreementDependencyValidationException(
                    message: "SubscriberAgreement dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberAgreementDependencyValidationException);

            return subscriberAgreementDependencyValidationException;
        }

        private SubscriberAgreementDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var subscriberAgreementDependencyException = 
                new SubscriberAgreementDependencyException(
                    message: "SubscriberAgreement dependency error occurred, please contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(subscriberAgreementDependencyException);

            return subscriberAgreementDependencyException;
        }

        private SubscriberAgreementServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var subscriberAgreementServiceException = 
                new SubscriberAgreementServiceException(
                    message: "SubscriberAgreement service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(subscriberAgreementServiceException);

            return subscriberAgreementServiceException;
        }
    }
}