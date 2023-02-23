using System;
using Xeptions;

namespace LHDS.Core.Models.Suppliers.Exceptions
{
    public class FailedSupplierStorageException : Xeption
    {
        public FailedSupplierStorageException(Exception innerException)
            : base(message: "Failed supplier storage error occurred, contact support.", innerException)
        { }
    }
}