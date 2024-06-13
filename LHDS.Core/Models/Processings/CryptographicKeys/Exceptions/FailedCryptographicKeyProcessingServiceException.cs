// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace LHDS.Core.Models.Processings.CryptographicKeys.Exceptions
{
    public class FailedCryptographicKeyProcessingServiceException : Xeption
    {
        public FailedCryptographicKeyProcessingServiceException(string message, Exception? innerException)
            : base(message, innerException)
        { }
    }
}
