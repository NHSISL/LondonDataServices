// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class InvalidAddressNormalisationArgumentException : Xeption
    {
        public InvalidAddressNormalisationArgumentException(string message)
            : base(message)
        { }
    }
}