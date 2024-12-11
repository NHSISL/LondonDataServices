// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressParsers.Exceptions
{
    public class AddressParserProcessingValidationException : Xeption
    {
        public AddressParserProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
