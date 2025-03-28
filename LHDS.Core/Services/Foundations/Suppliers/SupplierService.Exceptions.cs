// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Foundations.Suppliers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService
    {
        private delegate ValueTask<Supplier> ReturningSupplierFunction();
        private delegate ValueTask<IQueryable<Supplier>> ReturningSuppliersFunction();

        private async ValueTask<Supplier> TryCatch(ReturningSupplierFunction returningSupplierFunction)
        {
            try
            {
                return await returningSupplierFunction();
            }
            catch (NullSupplierException nullSupplierException)
            {
                throw await CreateAndLogValidationExceptionAsync(nullSupplierException);
            }
            catch (InvalidSupplierException invalidSupplierException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidSupplierException);
            }
            catch (SqlException sqlException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(
                        message: "Failed supplier storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSupplierStorageException);
            }
            catch (NotFoundSupplierException notFoundSupplierException)
            {
                throw await CreateAndLogValidationExceptionAsync(notFoundSupplierException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSupplierException =
                    new AlreadyExistsSupplierException(
                        message: "Supplier with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(alreadyExistsSupplierException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSupplierReferenceException =
                    new InvalidSupplierReferenceException(
                        message: "Invalid supplier reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw await CreateAndLogDependencyValidationExceptionAsync(invalidSupplierReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSupplierException = new LockedSupplierException(
                    message: "Locked supplier record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw await CreateAndLogDependencyValidationExceptionAsync(lockedSupplierException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(
                        message: "Failed supplier storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw await CreateAndLogDependencyExceptionAsync(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(
                        message: "Failed supplier service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSupplierServiceException);
            }
        }

        private async ValueTask<IQueryable<Supplier>> TryCatch(ReturningSuppliersFunction returningSuppliersFunction)
        {
            try
            {
                return await returningSuppliersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(
                        message: "Failed supplier storage error occurred, please contact support.",
                        innerException: sqlException);

                throw await CreateAndLogCriticalDependencyExceptionAsync(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(
                        message: "Failed supplier service error occurred, please contact support.",
                        innerException: exception);

                throw await CreateAndLogServiceExceptionAsync(failedSupplierServiceException);
            }
        }

        private async ValueTask<SupplierValidationException> CreateAndLogValidationExceptionAsync(Xeption exception)
        {
            var supplierValidationException = new SupplierValidationException(
                message: "Supplier validation errors occurred, please try again.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(supplierValidationException);

            return supplierValidationException;
        }

        private async ValueTask<SupplierDependencyException> 
            CreateAndLogCriticalDependencyExceptionAsync(Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(
                message: "Supplier dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogCriticalAsync(supplierDependencyException);

            return supplierDependencyException;
        }

        private async ValueTask<SupplierDependencyValidationException> 
            CreateAndLogDependencyValidationExceptionAsync(Xeption exception)
        {
            var supplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: exception);

            await this.loggingBroker.LogErrorAsync(supplierDependencyValidationException);

            return supplierDependencyValidationException;
        }

        private async ValueTask<SupplierDependencyException> CreateAndLogDependencyExceptionAsync(
            Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(
                message: "Supplier dependency error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(supplierDependencyException);

            return supplierDependencyException;
        }

        private async ValueTask<SupplierServiceException> CreateAndLogServiceExceptionAsync(
            Xeption exception)
        {
            var supplierServiceException = new SupplierServiceException(
                message: "Supplier service error occurred, please contact support.",
                innerException: exception);

            await this.loggingBroker.LogErrorAsync(supplierServiceException);

            return supplierServiceException;
        }
    }
}