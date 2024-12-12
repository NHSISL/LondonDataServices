// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class PostCodeNotFoundAddressMatcherProcessingServiceException : Xeption
    {
        public PostCodeNotFoundAddressMatcherProcessingServiceException(string message)
             : base(message)
        { }
    }
}
