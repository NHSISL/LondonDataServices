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
        private delegate IQueryable<Address> ReturningAddressesFunction();
        private delegate ValueTask<List<Address>> ReturningAddressListFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullAddressException nullAddressException)
            {
                throw CreateAndLogValidationException(nullAddressException);
            }
            catch (InvalidAddressException invalidAddressException)
            {
                throw CreateAndLogValidationException(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressStorageException);
            }
            catch (NotFoundAddressException notFoundAddressException)
            {
                throw CreateAndLogValidationException(notFoundAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressException =
                    new AlreadyExistsAddressException(
                        message: "Address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressReferenceException =
                    new InvalidAddressReferenceException(
                        message: "Invalid address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAddressException =
                    new LockedAddressException(
                        message: "Locked address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedAddressStorageException);
            }
            catch (AggregateException aggregateException)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed aggregate address service error occurred, please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressServiceException);
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
                throw CreateAndLogValidationException(nullAddressException);
            }
            catch (InvalidAddressException invalidAddressException)
            {
                throw CreateAndLogValidationException(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressStorageException);
            }
            catch (NotFoundAddressException notFoundAddressException)
            {
                throw CreateAndLogValidationException(notFoundAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAddressException =
                    new AlreadyExistsAddressException(
                        message: "Address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidAddressReferenceException =
                    new InvalidAddressReferenceException(
                        message: "Invalid address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAddressException =
                    new LockedAddressException(
                        message: "Locked address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressServiceException);
            }
        }

        private IQueryable<Address> TryCatch(ReturningAddressesFunction returningAddressesFunction)
        {
            try
            {
                return returningAddressesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressServiceException);
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
                throw CreateAndLogValidationException(invalidAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedAddressStorageException =
                    new FailedAddressStorageException(
                        message: "Failed address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedAddressServiceException =
                    new FailedAddressServiceException(
                        message: "Failed address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedAddressServiceException);
            }
        }

        private AddressValidationException CreateAndLogValidationException(Xeption exception)
        {
            var addressValidationException =
                new AddressValidationException(
                    message: "Address validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressValidationException);

            return addressValidationException;
        }

        private AddressDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var addressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(addressDependencyException);

            return addressDependencyException;
        }

        private AddressDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var addressDependencyValidationException =
                new AddressDependencyValidationException(
                    message: "Address dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(addressDependencyValidationException);

            return addressDependencyValidationException;
        }

        private AddressDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var addressDependencyException =
                new AddressDependencyException(
                    message: "Address dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressDependencyException);

            return addressDependencyException;
        }

        private AddressServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var addressServiceException =
                new AddressServiceException(
                    message: "Address service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(addressServiceException);

            return addressServiceException;
        }
    }
}