// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressNormalisations.Exceptions
{
    public class AddressNormalisationProcessingDependencyValidationException : Xeption
    {
        public AddressNormalisationProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
