using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.AddressLoadingAudits;
using LHDS.Core.Models.Foundations.AddressLoadingAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressLoadingAudits
{
    public partial class AddressLoadingAuditService
    {
        private delegate ValueTask<AddressLoadingAudit> ReturningAddressLoadingAuditFunction();

        private async ValueTask<AddressLoadingAudit> TryCatch(ReturningAddressLoadingAuditFunction returningAddressLoadingAuditFunction)
        {
            try
            {
                return await returningAddressLoadingAuditFunction();
            }
            catch (NullAddressLoadingAuditException nullAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(nullAddressLoadingAuditException);
            }
            catch (InvalidAddressLoadingAuditException invalidAddressLoadingAuditException)
            {
                throw CreateAndLogValidationException(invalidAddressLoadingAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressLoadingAuditStorageException =
                    new FailedAddressLoadingAuditStorageException(
                        message: "Failed addressLoadingAudit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressLoadingAuditStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressLoadingAuditException =
                    new AlreadyExistsAddressLoadingAuditException(
                        message: "AddressLoadingAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAddressLoadingAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressLoadingAuditReferenceException =
                    new InvalidAddressLoadingAuditReferenceException(
                        message: "Invalid addressLoadingAudit reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAddressLoadingAuditReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressLoadingAuditStorageException =
                    new FailedAddressLoadingAuditStorageException(
                        message: "Failed addressLoadingAudit storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedAddressLoadingAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressLoadingAuditServiceException =
                    new FailedAddressLoadingAuditServiceException(
                        message: "Failed addressLoadingAudit service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressLoadingAuditServiceException);
            }
        }

        private AddressLoadingAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressLoadingAuditValidationException =
                new AddressLoadingAuditValidationException(
                    message: "AddressLoadingAudit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditValidationException);

            return addressLoadingAuditValidationException;
        }

        private AddressLoadingAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressLoadingAuditDependencyException = 
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogCritical(addressLoadingAuditDependencyException);

            return addressLoadingAuditDependencyException;
        }

        private AddressLoadingAuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressLoadingAuditDependencyValidationException =
                new AddressLoadingAuditDependencyValidationException(
                    message: "AddressLoadingAudit dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditDependencyValidationException);

            return addressLoadingAuditDependencyValidationException;
        }

        private AddressLoadingAuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var addressLoadingAuditDependencyException = 
                new AddressLoadingAuditDependencyException(
                    message: "AddressLoadingAudit dependency error occurred, contact support.",
                    innerException: exception); 

            this.loggingBroker.LogError(addressLoadingAuditDependencyException);

            return addressLoadingAuditDependencyException;
        }

        private AddressLoadingAuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var addressLoadingAuditServiceException = 
                new AddressLoadingAuditServiceException(
                    message: "AddressLoadingAudit service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressLoadingAuditServiceException);

            return addressLoadingAuditServiceException;
        }
    }
}