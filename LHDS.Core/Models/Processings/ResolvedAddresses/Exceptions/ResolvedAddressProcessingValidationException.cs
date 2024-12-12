// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressProcessingValidationException : Xeption
    {
        public ResolvedAddressProcessingValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
