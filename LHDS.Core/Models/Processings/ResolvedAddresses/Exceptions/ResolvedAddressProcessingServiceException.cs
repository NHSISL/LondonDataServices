// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.ResolvedAddresses.Exceptions
{
    public class ResolvedAddressProcessingServiceException : Xeption
    {
        public ResolvedAddressProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
