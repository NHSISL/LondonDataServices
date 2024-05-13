// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Addresses.Exceptions
{
    public class AddressDependencyValidationException : Xeption
    {
        public AddressDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}