// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class InvalidAddressPartsNormalisationException : Xeption
    {
        public InvalidAddressPartsNormalisationException(string message)
            : base(message)
        { }
    }
}