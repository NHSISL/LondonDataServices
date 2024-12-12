// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class AddressMatcherProcessingValidationException : Xeption
    {
        public AddressMatcherProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
