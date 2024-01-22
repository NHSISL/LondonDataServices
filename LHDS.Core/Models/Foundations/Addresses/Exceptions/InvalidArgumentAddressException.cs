// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class InvalidArgumentAddressException : Xeption
    {
        public InvalidArgumentAddressException(string message)
            : base(message)
        { }
    }
}