// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class SupplierDependencyException : Xeption
    {
        public SupplierDependencyException(Xeption innerException) :
            base(message: "Supplier dependency error occurred, contact support.", innerException)
        { }
    }
}