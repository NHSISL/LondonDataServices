// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.AddressNormalisations.Exceptions
{
    public class FailedAddressNormalisationProcessingServiceException : Xeption
    {
        public FailedAddressNormalisationProcessingServiceException(string message, Exception? innerException)
          : base(message, innerException)
        { }
    }
}
