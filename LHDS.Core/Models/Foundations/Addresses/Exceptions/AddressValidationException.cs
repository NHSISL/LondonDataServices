// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class AddressValidationException : Xeption
    {
        public AddressValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}