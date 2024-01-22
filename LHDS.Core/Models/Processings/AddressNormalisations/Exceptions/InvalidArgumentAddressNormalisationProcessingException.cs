// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressNormalisations.Exceptions
{
    public class InvalidArgumentAddressNormalisationProcessingException : Xeption
    {
        public InvalidArgumentAddressNormalisationProcessingException(string message)
            : base(message)
        { }
    }
}