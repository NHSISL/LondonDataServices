// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class InvalidArgumentAddressParserException : Xeption
    {
        public InvalidArgumentAddressParserException(string message)
            : base(message)
        { }
    }
}