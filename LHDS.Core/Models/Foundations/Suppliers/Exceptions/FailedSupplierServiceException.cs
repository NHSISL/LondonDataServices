// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class FailedSupplierServiceException : Xeption
    {
        public FailedSupplierServiceException(Exception innerException)
            : base(message: "Failed supplier service occurred, please contact support", innerException) { }
    }
}