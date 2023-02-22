using LHDS.Core.Models.Suppliers;
using LHDS.Core.Models.Suppliers.Exceptions;

namespace LHDS.Core.Services.Foundations.Suppliers
{
    public partial class SupplierService
    {
        private void ValidateSupplierOnAdd(Supplier supplier)
        {
            ValidateSupplierIsNotNull(supplier);
        }

        private static void ValidateSupplierIsNotNull(Supplier supplier)
        {
            if (supplier is null)
            {
                throw new NullSupplierException();
            }
        }
    }
}