// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class InvalidArgumentAddressMatcherProcessingException : Xeption
    {
        public InvalidArgumentAddressMatcherProcessingException(string message)
            : base(message)
        { }
    }
}