// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Addresses.Exceptions
{
    public class InvalidArgumentAddressProcessingException : Xeption
    {
        public InvalidArgumentAddressProcessingException(string message)
            : base(message)
        { }
    }
}