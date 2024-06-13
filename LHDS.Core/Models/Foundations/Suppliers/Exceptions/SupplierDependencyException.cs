// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class SupplierDependencyException : Xeption
    {
        public SupplierDependencyException(string message, Xeption? innerException) :
            base(message, innerException)
        { }
    }
}