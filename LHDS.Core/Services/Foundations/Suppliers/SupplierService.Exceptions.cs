using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
    }
}