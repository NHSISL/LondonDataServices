using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using LHDS.Core.Models.Foundations.SubscriberAgreements;
using LHDS.Core.Models.Foundations.SubscriberAgreements.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.SubscriberAgreements
{
    public partial class SubscriberAgreementService
    {
        private delegate ValueTask<SubscriberAgreement> ReturningSubscriberAgreementFunction();

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
                        message: "Failed subscriberAgreement storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSubscriberAgreementStorageException);
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
                    message: "SubscriberAgreement dependency error occurred, contact support.",
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
    }
}