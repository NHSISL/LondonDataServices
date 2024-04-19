// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class InvalidCsvItemResolvedAddressParserException : Xeption
    {
        public InvalidCsvItemResolvedAddressParserException(string message)
            : base(message)
        { }
    }
}