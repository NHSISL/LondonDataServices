using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class FailedSupplierServiceException : Xeption
    {
        public FailedSupplierServiceException(Exception innerException)
            : base(message: "Failed supplier service occurred, please contact support", innerException)
        { }
    }
}