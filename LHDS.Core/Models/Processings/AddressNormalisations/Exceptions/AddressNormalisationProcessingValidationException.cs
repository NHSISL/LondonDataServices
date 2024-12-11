// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressNormalisations.Exceptions
{
    public class AddressNormalisationProcessingValidationException : Xeption
    {
        public AddressNormalisationProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
