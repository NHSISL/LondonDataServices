// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatcher.Exceptions
{
    public class PostCodeNotFoundAddressMatcherProcessingServiceException : Xeption
    {
        public PostCodeNotFoundAddressMatcherProcessingServiceException(string message)
             : base(message)
        { }
    }
}
