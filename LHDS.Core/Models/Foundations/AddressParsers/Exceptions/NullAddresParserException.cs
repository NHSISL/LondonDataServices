// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressParsers.Exceptions
{
    public class NullAddressParserException : Xeption
    {
        public NullAddressParserException(string message)
            : base(message)
        { }
    }
}