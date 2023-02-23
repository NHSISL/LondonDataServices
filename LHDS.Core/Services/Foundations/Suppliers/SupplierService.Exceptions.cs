// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using LHDS.Core.Models.Foundations.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService
    {
        private delegate ValueTask<Supplier> ReturningSupplierFunction();
        private delegate IQueryable<Supplier> ReturningSuppliersFunction();

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
                    new FailedSupplierStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedSupplierStorageException);
            }
            catch (NotFoundSupplierException notFoundSupplierException)
            {
                throw CreateAndLogValidationException(notFoundSupplierException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsSupplierException =
                    new AlreadyExistsSupplierException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsSupplierException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidSupplierReferenceException =
                    new InvalidSupplierReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidSupplierReferenceException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedSupplierException = new LockedSupplierException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyValidationException(lockedSupplierException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(exception);

                throw CreateAndLogServiceException(failedSupplierServiceException);
            }
        }

        private IQueryable<Supplier> TryCatch(ReturningSuppliersFunction returningSuppliersFunction)
        {
            try
            {
                return returningSuppliersFunction();
            }
            catch (SqlException sqlException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedSupplierStorageException);
            }
            catch (Exception exception)
            {
                var failedSupplierServiceException =
                    new FailedSupplierServiceException(exception);

                throw CreateAndLogServiceException(failedSupplierServiceException);
            }
        }

        private SupplierValidationException CreateAndLogValidationException(Xeption exception)
        {
            var supplierValidationException =
                new SupplierValidationException(exception);

            this.loggingBroker.LogError(supplierValidationException);

            return supplierValidationException;
        }

        private SupplierDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(exception);
            this.loggingBroker.LogCritical(supplierDependencyException);

            return supplierDependencyException;
        }

        private SupplierDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var supplierDependencyValidationException =
                new SupplierDependencyValidationException(exception);

            this.loggingBroker.LogError(supplierDependencyValidationException);

            return supplierDependencyValidationException;
        }

        private SupplierDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var supplierDependencyException = new SupplierDependencyException(exception);
            this.loggingBroker.LogError(supplierDependencyException);

            return supplierDependencyException;
        }

        private SupplierServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var supplierServiceException = new SupplierServiceException(exception);
            this.loggingBroker.LogError(supplierServiceException);

            return supplierServiceException;
        }
    }
}