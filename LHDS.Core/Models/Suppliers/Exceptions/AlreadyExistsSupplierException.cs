using System;
using Xeptions;

namespace LHDS.Core.Models.Suppliers.Exceptions
{
    public class AlreadyExistsSupplierException : Xeption
    {
        public AlreadyExistsSupplierException(Exception innerException)
            : base(message: "Supplier with the same Id already exists.", innerException)
        { }
    }
}