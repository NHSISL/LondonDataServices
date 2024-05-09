// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.Addresses.Exceptions
{
    public class AddressProcessingValidationException : Xeption
    {
        public AddressProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
