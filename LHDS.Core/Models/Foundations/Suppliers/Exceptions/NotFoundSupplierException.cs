// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Suppliers.Exceptions
{
    public class NotFoundSupplierException : Xeption
    {
        public NotFoundSupplierException(Guid supplierId)
            : base(message: $"Couldn't find supplier with supplierId: {supplierId}.")
        { }
    }
}