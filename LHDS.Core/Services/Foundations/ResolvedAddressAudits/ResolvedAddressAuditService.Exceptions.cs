// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressAudits.Exceptions;
using LHDS.Core.Models.Foundations.ResolvedAddressesAudits;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.ResolvedAddressAudits
{
    public partial class ResolvedAddressAuditService
    {
        private delegate ValueTask<ResolvedAddressAudit> ReturningResolvedAddressAuditFunction();
        private delegate ValueTask<IQueryable<ResolvedAddressAudit>> ReturningResolvedAddressAuditsFunction();

        private async ValueTask<ResolvedAddressAudit> TryCatch(ReturningResolvedAddressAuditFunction returningResolvedAddressAuditFunction)
        {
            try
            {
                return await returningResolvedAddressAuditFunction();
            }
            catch (NullResolvedAddressAuditException nullResolvedAddressAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullResolvedAddressAuditException);
            }
            catch (InvalidResolvedAddressAuditException invalidResolvedAddressAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidResolvedAddressAuditException);
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressAuditStorageException =
                    new FailedResolvedAddressAuditStorageException(
                        message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedResolvedAddressAuditStorageException);
            }
            catch (NotFoundResolvedAddressAuditException notFoundResolvedAddressAuditException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundResolvedAddressAuditException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsResolvedAddressAuditException =
                    new AlreadyExistsResolvedAddressAuditException(
                        message: "ResolvedAddressAudit with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsResolvedAddressAuditException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidResolvedAddressAuditReferenceException =
                    new InvalidResolvedAddressAuditReferenceException(
                        message: "Invalid resolvedAddressAudit reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidResolvedAddressAuditReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedResolvedAddressAuditException = new LockedResolvedAddressAuditException(
                    message: "Locked resolvedAddressAudit record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedResolvedAddressAuditException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedResolvedAddressAuditStorageException =
                    new FailedResolvedAddressAuditStorageException(
                        message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedResolvedAddressAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressAuditServiceException =
                    new FailedResolvedAddressAuditServiceException(
                        message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressAuditServiceException);
            }
        }

        private async ValueTask<IQueryable<ResolvedAddressAudit>> TryCatch(ReturningResolvedAddressAuditsFunction returningResolvedAddressAuditsFunction)
        {
            try
            {
                return await returningResolvedAddressAuditsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedResolvedAddressAuditStorageException =
                    new FailedResolvedAddressAuditStorageException(
                        message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedResolvedAddressAuditStorageException);
            }
            catch (Exception exception)
            {
                var failedResolvedAddressAuditServiceException =
                    new FailedResolvedAddressAuditServiceException(
                        message: "Failed resolvedAddressAudit service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedResolvedAddressAuditServiceException);
            }
        }

        private async ValueTask<ResolvedAddressAuditValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressAuditValidationException =
                new ResolvedAddressAuditValidationException(
                    message: "ResolvedAddressAudit validation errors occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressAuditValidationException);

            return resolvedAddressAuditValidationException;
        }

        private async ValueTask<ResolvedAddressAuditDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var resolvedAddressAuditDependencyException = new ResolvedAddressAuditDependencyException(
                message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(resolvedAddressAuditDependencyException);

            return resolvedAddressAuditDependencyException;
        }

        private async ValueTask<ResolvedAddressAuditDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var resolvedAddressAuditDependencyValidationException =
                new ResolvedAddressAuditDependencyValidationException(
                    message: "ResolvedAddressAudit dependency validation occurred, please try again.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressAuditDependencyValidationException);

            return resolvedAddressAuditDependencyValidationException;
        }

        private async ValueTask<ResolvedAddressAuditDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressAuditDependencyException = new ResolvedAddressAuditDependencyException(
                message: "ResolvedAddressAudit dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressAuditDependencyException);

            return resolvedAddressAuditDependencyException;
        }

        private async ValueTask<ResolvedAddressAuditServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var resolvedAddressAuditServiceException = new ResolvedAddressAuditServiceException(
                message: "ResolvedAddressAudit service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(resolvedAddressAuditServiceException);

            return resolvedAddressAuditServiceException;
        }
    }
}