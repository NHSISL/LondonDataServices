// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class InvalidResolvedAddressProcessingException : Xeption
    {
        public InvalidResolvedAddressProcessingException(string message)
            : base(message)
        { }
    }
}