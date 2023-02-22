using System.Threading.Tasks;
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
        }

        private SupplierValidationException CreateAndLogValidationException(Xeption exception)
        {
            var supplierValidationException =
                new SupplierValidationException(exception);

            this.loggingBroker.LogError(supplierValidationException);

            return supplierValidationException;
        }
    }
}