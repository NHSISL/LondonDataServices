// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class NotFoundAddressException : Xeption
    {
        public NotFoundAddressException(Guid addressId)
            : base(message: $"Couldn't find address with addressId: {addressId}.")
        { }
    }
}