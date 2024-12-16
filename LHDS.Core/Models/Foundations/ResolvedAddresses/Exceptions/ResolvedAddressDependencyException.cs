// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressDependencyException : Xeption
    {
        public ResolvedAddressDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}