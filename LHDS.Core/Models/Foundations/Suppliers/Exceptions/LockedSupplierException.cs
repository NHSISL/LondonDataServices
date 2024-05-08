// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class LockedSupplierException : Xeption
    {
        public LockedSupplierException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}