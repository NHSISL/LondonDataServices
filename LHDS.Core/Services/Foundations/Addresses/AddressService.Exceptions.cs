// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.Addresses;
using LHDS.Core.Models.Foundations.Addresses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Addresses
{
    public partial class AddressService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<Address> ReturningAddressFunction();
        private delegate ValueTask<IQueryable<Address>> ReturningAddressesFunction();
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullAddressException nullAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAddressException);
            }
            catch (InvalidAddressException invalidAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (NotFoundAddressException notFoundAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressException =
                    new AlreadyExistsAddressException(
                        message: "Address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressReferenceException =
                    new InvalidAddressReferenceException(
                        message: "Invalid address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAddressException =
                    new LockedAddressException(
                        message: "Locked address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed aggregate address service error occurred, please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressServiceException);
            }
        }

        private async ValueTask<Address> TryCatch(ReturningAddressFunction returningAddressFunction)
        {
            try
            {
                return await returningAddressFunction();
            }
            catch (NullAddressException nullAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullAddressException);
            }
            catch (InvalidAddressException invalidAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (NotFoundAddressException notFoundAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressException =
                    new AlreadyExistsAddressException(
                        message: "Address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressReferenceException =
                    new InvalidAddressReferenceException(
                        message: "Invalid address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAddressException =
                    new LockedAddressException(
                        message: "Locked address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressServiceException);
            }
        }

        private async ValueTask<IQueryable<Address>> TryCatch(ReturningAddressesFunction returningAddressesFunction)
        {
            try
            {
                return await returningAddressesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressServiceException);
            }
        }

        private async ValueTask<List<Address>> TryCatch(ReturningAddressListFunction returningAddressListFunction)
        {
            try
            {
                return await returningAddressListFunction();
            }
            catch (InvalidAddressException invalidAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedAddressServiceException);
            }
        }

        private async ValueTask<AddressValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var addressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressValidationException);

            return addressValidationException;
        }

        private async ValueTask<AddressDependencyException> CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var addressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(addressDependencyException);

            return addressDependencyException;
        }

        private async ValueTask<AddressDependencyValidationException> CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var addressDependencyValidationException =
                new AddressDependencyValidationException(
                    message: "Address dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressDependencyValidationException);

            return addressDependencyValidationException;
        }

        private async ValueTask<AddressDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var addressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressDependencyException);

            return addressDependencyException;
        }

        private async ValueTask<AddressServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var addressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(addressServiceException);

            return addressServiceException;
        }
    }
}