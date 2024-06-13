// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Processings.CryptographicKeys.Exceptions
{
    public class CryptographicKeyValidationProcessingException : Xeption
    {
        public CryptographicKeyValidationProcessingException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
