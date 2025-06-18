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
                        message: "Ingestion tracking audit with the same Id already exists.",
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
            catch (Exception exception)
            {
                var failedResolvedAddressServiceException =
                    new FailedResolvedAddressServiceException(
                        message: "Failed resolved address service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressServiceException);
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
                        message: "Ingestion tracking audit with the same Id already exists.",
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

        private async ValueTask<ResolvedAddressValidationException> 
            CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressValidationException =
                new ResolvedAddressValidationException(
                    message: "Ingestion tracking audit validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressValidationException);

            return resolvedAddressValidationException;
        }

        private async ValueTask<ResolvedAddressDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Ingestion tracking audit dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private async ValueTask<ResolvedAddressDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressDependencyValidationException =
                new ResolvedAddressDependencyValidationException(
                    message: "Ingestion tracking audit dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressDependencyValidationException);

            return resolvedAddressDependencyValidationException;
        }

        private async ValueTask<ResolvedAddressDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressDependencyException =
                new ResolvedAddressDependencyException(
                    message: "Ingestion tracking audit dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressDependencyException);

            return resolvedAddressDependencyException;
        }

        private async ValueTask<ResolvedAddressServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressServiceException =
                new ResolvedAddressServiceException(
                    message: "Ingestion tracking audit service error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressServiceException);

            return resolvedAddressServiceException;
        }
    }
}