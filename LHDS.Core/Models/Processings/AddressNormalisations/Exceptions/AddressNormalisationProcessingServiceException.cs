// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.AddressNormalisations.Exceptions
{
    public class AddressNormalisationProcessingServiceException : Xeption
    {
        public AddressNormalisationProcessingServiceException(string message, Xeption? innerException)
          : base(message, innerException)
        { }
    }
}
