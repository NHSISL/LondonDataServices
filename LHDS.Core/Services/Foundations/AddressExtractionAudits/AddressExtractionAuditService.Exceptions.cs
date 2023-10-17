using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Foundations.AddressExtractionAudits;
using LHDS.Core.Models.Foundations.AddressExtractionAudits.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressExtractionAudits
{
    public partial class AddressExtractionAuditService
    {
        private delegate ValueTask<AddressExtractionAudit> ReturningAddressExtractionAuditFunction();
        private delegate IQueryable<AddressExtractionAudit> ReturningAddressExtractionAuditsFunction();

        private async ValueTask<AddressExtractionAudit> TryCatch(ReturningAddressExtractionAuditFunction returningAddressExtractionAuditFunction)
        {
            try
            {
                return await returningAddressExtractionAuditFunction();
            }
            catch (NullAddressExtractionAuditException nullAddressExtractionAuditException)
            {
                throw CreateAndLogValidationException(nullAddressExtractionAuditException);
            }
            catch (InvalidAddressExtractionAuditException invalidAddressExtractionAuditException)
            {
                throw CreateAndLogValidationException(invalidAddressExtractionAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressExtractionAuditStorageException =
                    new FailedAddressExtractionAuditStorageException(
                        message: "Failed addressExtractionAudit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressExtractionAuditStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressExtractionAuditException =
                    new AlreadyExistsAddressExtractionAuditException(
                        message: "AddressExtractionAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAddressExtractionAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressExtractionAuditReferenceException =
                    new InvalidAddressExtractionAuditReferenceException(
                        message: "Invalid addressExtractionAudit reference error occurred.", 
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAddressExtractionAuditReferenceException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressExtractionAuditStorageException =
                    new FailedAddressExtractionAuditStorageException(
                        message: "Failed addressExtractionAudit storage error occurred, contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedAddressExtractionAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressExtractionAuditServiceException =
                    new FailedAddressExtractionAuditServiceException(
                        message: "Failed addressExtractionAudit service occurred, please contact support", 
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressExtractionAuditServiceException);
            }
        }

        private IQueryable<AddressExtractionAudit> TryCatch(ReturningAddressExtractionAuditsFunction returningAddressExtractionAuditsFunction)
        {
            try
            {
                return returningAddressExtractionAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAddressExtractionAuditStorageException =
                    new FailedAddressExtractionAuditStorageException(
                        message: "Failed addressExtractionAudit storage error occurred, contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressExtractionAuditStorageException);
            }
        }

        private AddressExtractionAuditValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressExtractionAuditValidationException =
                new AddressExtractionAuditValidationException(
                    message: "AddressExtractionAudit validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionAuditValidationException);

            return addressExtractionAuditValidationException;
        }

        private AddressExtractionAuditDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressExtractionAuditDependencyException = 
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(addressExtractionAuditDependencyException);

            return addressExtractionAuditDependencyException;
        }

        private AddressExtractionAuditDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressExtractionAuditDependencyValidationException =
                new AddressExtractionAuditDependencyValidationException(
                    message: "AddressExtractionAudit dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionAuditDependencyValidationException);

            return addressExtractionAuditDependencyValidationException;
        }

        private AddressExtractionAuditDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var addressExtractionAuditDependencyException = 
                new AddressExtractionAuditDependencyException(
                    message: "AddressExtractionAudit dependency error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionAuditDependencyException);

            return addressExtractionAuditDependencyException;
        }

        private AddressExtractionAuditServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var addressExtractionAuditServiceException = 
                new AddressExtractionAuditServiceException(
                    message: "AddressExtractionAudit service error occurred, contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressExtractionAuditServiceException);

            return addressExtractionAuditServiceException;
        }
    }
}