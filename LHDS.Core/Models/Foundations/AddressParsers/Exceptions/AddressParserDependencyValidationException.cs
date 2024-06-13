// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class AddressParserDependencyValidationException : Xeption
    {
        public AddressParserDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
