// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class FailedSupplierServiceException : Xeption
    {
        public FailedSupplierServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}