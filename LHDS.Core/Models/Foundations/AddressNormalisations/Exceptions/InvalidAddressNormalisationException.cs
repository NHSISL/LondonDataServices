// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class InvalidAddressNormalisationException : Xeption
    {
        public InvalidAddressNormalisationException(string message)
            : base(message)
        { }
    }
}