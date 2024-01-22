// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class InvalidSupplierException : Xeption
    {
        public InvalidSupplierException(string message)
            : base(message)
        { }
    }
}