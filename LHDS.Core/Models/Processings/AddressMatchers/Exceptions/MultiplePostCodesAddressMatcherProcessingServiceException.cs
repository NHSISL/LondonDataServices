// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressMatchers.Exceptions
{
    public class MultiplePostCodesAddressMatcherProcessingServiceException : Xeption
    {
        public MultiplePostCodesAddressMatcherProcessingServiceException(string message)
             : base(message)
        { }
    }
}
