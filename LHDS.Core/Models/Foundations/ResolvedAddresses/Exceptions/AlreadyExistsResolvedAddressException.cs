// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class AlreadyExistsResolvedAddressException : Xeption
    {
        public AlreadyExistsResolvedAddressException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}