using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class SupplierServiceException : Xeption
    {
        public SupplierServiceException(Exception innerException)
            : base(message: "Supplier service error occurred, contact support.", innerException)
        { }
    }
}