// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CryptographicKeys.Exceptions
{
    public class CryptographicKeyProcessingServiceException : Xeption
    {
        public CryptographicKeyProcessingServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
