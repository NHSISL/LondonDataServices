// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.ResolvedAddressParsers.Exceptions
{
    public class ResolvedAddressParserValidationException : Xeption
    {
        public ResolvedAddressParserValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}