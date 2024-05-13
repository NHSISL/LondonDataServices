// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class AddressParserDependencyException : Xeption
    {
        public AddressParserDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}