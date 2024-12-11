// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class NotFoundResolvedAddressException : Xeption
    {
        public NotFoundResolvedAddressException(Guid resolvedAddressId)
            : base(message: $"Couldn't find resolvedAddress with resolvedAddressId: {resolvedAddressId}.")
        { }
    }
}