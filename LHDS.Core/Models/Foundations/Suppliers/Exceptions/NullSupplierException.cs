using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class NullSupplierException : Xeption
    {
        public NullSupplierException()
            : base(message: "Supplier is null.")
        { }
    }
}