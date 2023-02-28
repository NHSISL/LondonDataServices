using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class SupplierDependencyValidationException : Xeption
    {
        public SupplierDependencyValidationException(Xeption innerException)
            : base(message: "Supplier dependency validation occurred, please try again.", innerException)
        { }
    }
}