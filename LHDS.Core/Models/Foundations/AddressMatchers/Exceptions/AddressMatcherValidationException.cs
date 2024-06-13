// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressMatchers.Exceptions
{
    public class AddressMatcherValidationException : Xeption
    {
        public AddressMatcherValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
