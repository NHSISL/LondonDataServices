// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class NullSupplierException : Xeption
    {
        public NullSupplierException()
            : base(message: "Supplier is null.") { }
    }
}