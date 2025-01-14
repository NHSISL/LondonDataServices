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
                throw CreateAndLogValidationException(nullSupplierException);
            }
            catch (InvalidSupplierException invalidSupplierException)
            {
                throw CreateAndLogValidationException(invalidSupplierException);
            }
            catch (SqlException sqlException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(
                        message: "Failed supplier storage error occurred, please contact support.",
                        innerException: sqlException);

                throw CreateAndLogCriticalDependencyException(failedSupplierStorageException);
            }
            catch (NotFoundSupplierException notFoundSupplierException)
            {
                throw CreateAndLogValidationException(notFoundSupplierException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSupplierException =
                    new AlreadyExistsSupplierException(
                        message: "Supplier with the same Id already exists.",
                        innerException: duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSupplierException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSupplierReferenceException =
                    new InvalidSupplierReferenceException(
                        message: "Invalid supplier reference error occurred.",
                        innerException: foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidSupplierReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSupplierException = new LockedSupplierException(
                    message: "Locked supplier record exception, please try again later",
                    innerException: dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedSupplierException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(
                        message: "Failed supplier storage error occurred, please contact support.",
                        innerException: databaseUpdateException);

                throw CreateAndLogDependencyException(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(
                        message: "Failed supplier service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSupplierServiceException);
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

                throw CreateAndLogCriticalDependencyException(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(
                        message: "Failed supplier service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedSupplierServiceException);
            }
        }

        private SupplierValidationException CreateAndLogValidationException(Xeption exception)
        {
            var supplierValidationException = new SupplierValidationException(
                message: "Supplier validation errors occurred, please try again.",
                innerException: exception);

            this.loggingBroker.LogError(supplierValidationException);

            return supplierValidationException;
        }

        private SupplierDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(
                message: "Supplier dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogCritical(supplierDependencyException);

            return supplierDependencyException;
        }

        private SupplierDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var supplierDependencyValidationException =
                new SupplierDependencyValidationException(
                    message: "Supplier dependency error occurred, please contact support.",
                    innerException: exception);

            this.loggingBroker.LogError(supplierDependencyValidationException);

            return supplierDependencyValidationException;
        }

        private SupplierDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(
                message: "Supplier dependency error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(supplierDependencyException);

            return supplierDependencyException;
        }

        private SupplierServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var supplierServiceException = new SupplierServiceException(
                message: "Supplier service error occurred, please contact support.",
                innerException: exception);

            this.loggingBroker.LogError(supplierServiceException);

            return supplierServiceException;
        }
    }
}