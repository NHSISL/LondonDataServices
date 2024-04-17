// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class NullResolvedAddressParserException : Xeption
    {
        public NullResolvedAddressParserException(string message)
            : base(message)
        { }
    }
}