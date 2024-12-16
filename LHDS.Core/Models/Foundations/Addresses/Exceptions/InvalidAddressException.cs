// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class InvalidAddressException : Xeption
    {
        public InvalidAddressException(string message)
            : base(message)
        { }
    }
}