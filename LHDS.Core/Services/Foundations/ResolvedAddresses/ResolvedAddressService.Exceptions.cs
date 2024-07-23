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
        private delegate IQueryable<ResolvedAddress> ReturningResolvedAddressesFunction();

        private async ValueTask<ResolvedAddress> TryCatch(ReturningResolvedAddressFunction returningResolvedAddressFunction)
        {
            try
            {
                return await returningResolvedAddressFunction();
            }
            catch (NullResolvedAddressException nullResolvedAddressException)
            {
                throw CreateAndLogValidationException(nullResolvedAddressException);
            }
            catch (InvalidResolvedAddressException invalidResolvedAddressException)
            {
                throw CreateAndLogValidationException(invalidResolvedAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedResolvedAddressStorageException);
            }
            catch (NotFoundResolvedAddressException notFoundResolvedAddressException)
            {
                throw CreateAndLogValidationException(notFoundResolvedAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressException =
                    new AlreadyExistsResolvedAddressException(
                        message: "Resolved address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsResolvedAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidResolvedAddressReferenceException =
                    new InvalidResolvedAddressReferenceException(
                        message: "Invalid resolved address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidResolvedAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedResolvedAddressException =
                    new LockedResolvedAddressException(
                        message: "Locked resolved address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedResolvedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedResolvedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressServiceException);
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
                throw CreateAndLogValidationException(nullResolvedAddressException);
            }
            catch (InvalidResolvedAddressException invalidResolvedAddressException)
            {
                throw CreateAndLogValidationException(invalidResolvedAddressException);
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedResolvedAddressStorageException);
            }
            catch (NotFoundResolvedAddressException notFoundResolvedAddressException)
            {
                throw CreateAndLogValidationException(notFoundResolvedAddressException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressException =
                    new AlreadyExistsResolvedAddressException(
                        message: "Resolved address with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsResolvedAddressException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidResolvedAddressReferenceException =
                    new InvalidResolvedAddressReferenceException(
                        message: "Invalid resolved address reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidResolvedAddressReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedResolvedAddressException =
                    new LockedResolvedAddressException(
                        message: "Locked resolved address record exception, please try again later",
                        innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedResolvedAddressException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedResolvedAddressStorageException);
            }
            catch (AggregateException aggregateException)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed aggregate resolved address service error occurred, please contact support.",
                        innerException: aggregateException);

                throw CreateAndLogServiceException(failedResolvedAddressServiceException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressServiceException);
            }
        }

        private IQueryable<ResolvedAddress> TryCatch(ReturningResolvedAddressesFunction returningResolvedAddressesFunction)
        {
            try
            {
                return returningResolvedAddressesFunction();
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressStorageException =
                    new FailedResolvedAddressStorageException(
                        message: "Failed resolved address storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedResolvedAddressStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedResolvedAddressServiceException);
            }
        }

        private ResolvedAddressValidationException CreateAndLogValidationException(Xeption exception)
        {
            var resolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Resolved address validation errors occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressValidationException);

            return resolvedAddressValidationException;
        }

        private ResolvedAddressDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogCritical(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private ResolvedAddressDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var resolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "Resolved address dependency validation occurred, please try again.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressDependencyValidationException);

            return resolvedAddressDependencyValidationException;
        }

        private ResolvedAddressDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Resolved address dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private ResolvedAddressServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var resolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Resolved address service error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(resolvedAddressServiceException);

            return resolvedAddressServiceException;
        }
    }
}