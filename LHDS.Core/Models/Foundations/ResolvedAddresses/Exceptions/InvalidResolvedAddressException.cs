// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class InvalidResolvedAddressException : Xeption
    {
        public InvalidResolvedAddressException(string message)
            : base(message)
        { }
    }
}