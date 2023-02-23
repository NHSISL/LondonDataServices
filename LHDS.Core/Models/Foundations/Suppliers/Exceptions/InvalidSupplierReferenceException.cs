using System;
using Xeptions;

namespace LHDS.Core.Models.Suppliers.Exceptions
{
    public class InvalidSupplierReferenceException : Xeption
    {
        public InvalidSupplierReferenceException(Exception innerException)
            : base(message: "Invalid supplier reference error occurred.", innerException) { }
    }
}