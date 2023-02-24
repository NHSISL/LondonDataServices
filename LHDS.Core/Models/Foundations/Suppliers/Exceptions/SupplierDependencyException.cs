using Xeptions;

namespace LHDS.Core.Models.Suppliers.Exceptions
{
    public class SupplierDependencyException : Xeption
    {
        public SupplierDependencyException(Xeption innerException) :
            base(message: "Supplier dependency error occurred, contact support.", innerException)
        { }
    }
}