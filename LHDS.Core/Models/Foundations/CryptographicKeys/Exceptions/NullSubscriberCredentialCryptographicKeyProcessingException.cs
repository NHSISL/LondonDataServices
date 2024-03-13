// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.CryptographicKeys.Exceptions
{
    public class NullSubscriberCredentialCryptographicKeyProcessingException : Xeption
    {
        public NullSubscriberCredentialCryptographicKeyProcessingException(string message)
            : base(message)
        { }
    }
}