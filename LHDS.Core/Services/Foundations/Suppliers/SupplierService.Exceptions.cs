using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;
using Xeptions;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService
    {
        private delegate ValueTask<Supplier> ReturningSupplierFunction();

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
            catch (DbUpdateException databaseUpdateException)
            {
                var failedSupplierStorageException =
                    new FailedSupplierStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedSupplierStorageException);
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
    }
}