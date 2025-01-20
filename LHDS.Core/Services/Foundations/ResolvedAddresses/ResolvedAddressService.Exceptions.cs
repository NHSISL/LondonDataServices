// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddresses;
using LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddresses
{
    public partial class ResolvedAddressService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<ResolvedAddress> ReturningResolvedAddressFunction();
        private delegate ValueTask<IQueryable<ResolvedAddress>> ReturningResolvedAddressesFunction();

        private async ValueTask<ResolvedAddress> TryCatch(ReturningResolvedAddressFunction returningResolvedAddressFunction)
        {
            try
            {
                return await returningResolvedAddressFunction();
            }
            catch (NullResolvedAddressException nullResolvedAddressException)
            {
                throw CreateAndLogValidationExceptionAsync(nullResolvedAddressException);
            }
            catch (InvalidResolvedAddressException invalidResolvedAddressException)
            {
                throw CreateAndLogValidationExceptionAsync(invalidResolvedAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyExceptionAsync(failedResolvedAddressStorageException);
            }
            catch (NotFoundResolvedAddressException notFoundResolvedAddressException)
            {
                throw CreateAndLogValidationExceptionAsync(notFoundResolvedAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressException =
                    new AlreadyExistsResolvedAddressException(
                        message: "Resolved address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationExceptionAsync(alreadyExistsResolvedAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidResolvedAddressReferenceException =
                    new InvalidResolvedAddressReferenceException(
                        message: "Invalid resolved address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationExceptionAsync(invalidResolvedAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedResolvedAddressException =
                    new LockedResolvedAddressException(
                        message: "Locked resolved address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationExceptionAsync(lockedResolvedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyExceptionAsync(failedResolvedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceExceptionAsync(failedResolvedAddressServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullResolvedAddressException nullResolvedAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullResolvedAddressException);
            }
            catch (InvalidResolvedAddressException invalidResolvedAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidResolvedAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedResolvedAddressStorageException);
            }
            catch (NotFoundResolvedAddressException notFoundResolvedAddressException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundResolvedAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressException =
                    new AlreadyExistsResolvedAddressException(
                        message: "Resolved address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsResolvedAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidResolvedAddressReferenceException =
                    new InvalidResolvedAddressReferenceException(
                        message: "Invalid resolved address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidResolvedAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedResolvedAddressException =
                    new LockedResolvedAddressException(
                        message: "Locked resolved address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedResolvedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedResolvedAddressStorageException);
            }
            catch (AggregateException aggregateException)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed aggregate resolved address service error occurred, please contact support.",
                        innerException: aggregateException);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressServiceException);
            }
        }

        private async ValueTask<IQueryable<ResolvedAddress>>
            TryCatch(ReturningResolvedAddressesFunction returningResolvedAddressesFunction)
        {
            try
            {
                return await returningResolvedAddressesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedResolvedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressServiceException);
            }
        }

        private ResolvedAddressValidationException CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressValidationException);

            return resolvedAddressValidationException;
        }

        private ResolvedAddressDependencyException CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private ResolvedAddressDependencyValidationException CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressDependencyValidationException);

            return resolvedAddressDependencyValidationException;
        }

        private ResolvedAddressDependencyException CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private ResolvedAddressServiceException CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressServiceException);

            return resolvedAddressServiceException;
        }
    }
}