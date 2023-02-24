using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class LockedSupplierException : Xeption
    {
        public LockedSupplierException(Exception innerException)
            : base(message: "Locked supplier record exception, please try again later", innerException)
        {
        }
    }
}