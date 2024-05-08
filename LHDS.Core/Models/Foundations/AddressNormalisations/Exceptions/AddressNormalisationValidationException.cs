// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.AddressNormalisations.Exceptions
{
    public class AddressNormalisationValidationException : Xeption
    {
        public AddressNormalisationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}