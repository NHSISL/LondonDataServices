// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class InvalidArgumentResolvedAddressParserException : Xeption
    {
        public InvalidArgumentResolvedAddressParserException(string message)
            : base(message)
        { }
    }
}