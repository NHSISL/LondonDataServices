// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decryptions.Exceptions
{
    public class InvalidArgumentDecryptionCoordinationException : Xeption
    {
        public InvalidArgumentDecryptionCoordinationException(string message)
            : base(message)
        { }
    }
}