using Xeptions;

namespace LHDS.Core.Models.Suppliers.Exceptions
{
    public class SupplierValidationException : Xeption
    {
        public SupplierValidationException(Xeption innerException)
            : base(message: "Supplier validation errors occurred, please try again.",
                  innerException)
        { }
    }
}