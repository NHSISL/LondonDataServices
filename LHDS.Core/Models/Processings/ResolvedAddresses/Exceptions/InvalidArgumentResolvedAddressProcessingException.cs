// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class InvalidArgumentResolvedAddressProcessingException : Xeption
    {
        public InvalidArgumentResolvedAddressProcessingException(string message)
            : base(message)
        { }
    }
}