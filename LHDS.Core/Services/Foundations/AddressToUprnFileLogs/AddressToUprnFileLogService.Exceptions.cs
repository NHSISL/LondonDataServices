// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs;
using LHDS.Core.Models.Foundations.AddressToUprnFileLogs.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.AddressToUprnFileLogs
{
    public partial class AddressToUprnFileLogService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<AddressToUprnFileLog> ReturningAddressToUprnFileLogFunction();
        private delegate ValueTask<IQueryable<AddressToUprnFileLog>> ReturningAddressToUprnFileLogsFunction();

        private async ValueTask<AddressToUprnFileLog> TryCatch(
            ReturningAddressToUprnFileLogFunction returningAddressToUprnFileLogFunction)
        {
            try
            {
                return await returningAddressToUprnFileLogFunction();
            }
            catch (NullAddressToUprnFileLogException nullAddressToUprnFileLogException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAddressToUprnFileLogException);
            }
            catch (InvalidAddressToUprnFileLogException invalidAddressToUprnFileLogException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidAddressToUprnFileLogException);
            }
            catch (NotFoundAddressToUprnFileLogException notFoundAddressToUprnFileLogException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundAddressToUprnFileLogException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressToUprnFileLogStorageException =
                    new FailedAddressToUprnFileLogStorageException(
                        message: "Failed address to UPRN file log storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressToUprnFileLogStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressToUprnFileLogException =
                    new AlreadyExistsAddressToUprnFileLogException(
                        message: "Address to UPRN file log with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(
                    alreadyExistsAddressToUprnFileLogException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressToUprnFileLogReferenceException =
                    new InvalidAddressToUprnFileLogReferenceException(
                        message: "Invalid address to UPRN file log reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(
                    invalidAddressToUprnFileLogReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAddressToUprnFileLogException =
                    new LockedAddressToUprnFileLogException(
                        message: "Locked address to UPRN file log record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedAddressToUprnFileLogException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressToUprnFileLogStorageException =
                    new FailedAddressToUprnFileLogStorageException(
                        message: "Failed address to UPRN file log storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedAddressToUprnFileLogStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressToUprnFileLogServiceException =
                    new FailedAddressToUprnFileLogServiceException(
                        message: "Failed address to UPRN file log service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressToUprnFileLogServiceException);
            }
        }

        private async ValueTask<IQueryable<AddressToUprnFileLog>> TryCatch(
            ReturningAddressToUprnFileLogsFunction returningAddressToUprnFileLogsFunction)
        {
            try
            {
                return await returningAddressToUprnFileLogsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAddressToUprnFileLogStorageException =
                    new FailedAddressToUprnFileLogStorageException(
                        message: "Failed address to UPRN file log storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressToUprnFileLogStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressToUprnFileLogServiceException =
                    new FailedAddressToUprnFileLogServiceException(
                        message: "Failed address to UPRN file log service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressToUprnFileLogServiceException);
            }
        }

        private async ValueTask<AddressToUprnFileLogValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var addressToUprnFileLogValidationException =
                new AddressToUprnFileLogValidationException(
                    message: "Address to UPRN file log validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnFileLogValidationException);

            return addressToUprnFileLogValidationException;
        }

        private async ValueTask<AddressToUprnFileLogDependencyException>
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var addressToUprnFileLogDependencyException =
                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(addressToUprnFileLogDependencyException);

            return addressToUprnFileLogDependencyException;
        }

        private async ValueTask<AddressToUprnFileLogDependencyValidationException>
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressToUprnFileLogDependencyValidationException =
                new AddressToUprnFileLogDependencyValidationException(
                    message: "Address to UPRN file log dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnFileLogDependencyValidationException);

            return addressToUprnFileLogDependencyValidationException;
        }

        private async ValueTask<AddressToUprnFileLogDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var addressToUprnFileLogDependencyException =
                new AddressToUprnFileLogDependencyException(
                    message: "Address to UPRN file log dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnFileLogDependencyException);

            return addressToUprnFileLogDependencyException;
        }

        private async ValueTask<AddressToUprnFileLogServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var addressToUprnFileLogServiceException =
                new AddressToUprnFileLogServiceException(
                    message: "Address to UPRN file log service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressToUprnFileLogServiceException);

            return addressToUprnFileLogServiceException;
        }
    }
}
