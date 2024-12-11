// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressDependencyValidationException : Xeption
    {
        public ResolvedAddressDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}