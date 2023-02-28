using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class InvalidSupplierException : Xeption
    {
        public InvalidSupplierException()
            : base(message: "Invalid supplier. Please correct the errors and try again.")
        { }
    }
}