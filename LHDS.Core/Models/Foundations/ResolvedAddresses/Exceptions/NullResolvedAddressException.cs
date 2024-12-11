// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class NullResolvedAddressException : Xeption
    {
        public NullResolvedAddressException(string message)
            : base(message)
        { }
    }
}