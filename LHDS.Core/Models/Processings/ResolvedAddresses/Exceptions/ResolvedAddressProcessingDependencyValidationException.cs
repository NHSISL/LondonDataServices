// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressProcessingDependencyValidationException : Xeption
    {
        public ResolvedAddressProcessingDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
