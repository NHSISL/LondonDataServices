// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CryptographicKeys.Exceptions
{
    public class CryptographicKeyProcessingDependencyException : Xeption
    {
        public CryptographicKeyProcessingDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
