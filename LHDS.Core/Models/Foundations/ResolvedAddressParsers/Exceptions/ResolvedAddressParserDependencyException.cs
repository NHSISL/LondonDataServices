// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class ResolvedAddressParserDependencyException : Xeption
    {
        public ResolvedAddressParserDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}