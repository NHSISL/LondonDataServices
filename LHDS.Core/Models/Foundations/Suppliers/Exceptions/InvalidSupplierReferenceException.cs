// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class InvalidSupplierReferenceException : Xeption
    {
        public InvalidSupplierReferenceException(Exception innerException)
            : base(message: "Invalid supplier reference error occurred.", innerException) { }
    }
}