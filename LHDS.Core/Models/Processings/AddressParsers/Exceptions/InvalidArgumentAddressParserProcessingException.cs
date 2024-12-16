// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressParsers.Exceptions
{
    public class InvalidArgumentAddressParserProcessingException : Xeption
    {
        public InvalidArgumentAddressParserProcessingException(string message)
            : base(message)
        { }
    }
}