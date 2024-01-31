// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressMatchers.Exceptions
{
    public class InvalidArgumentAddressMatcherException : Xeption
    {
        public InvalidArgumentAddressMatcherException(string message)
            : base(message)
        { }
    }
}