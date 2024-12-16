// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressValidationException : Xeption
    {
        public ResolvedAddressValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}