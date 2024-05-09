// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.Decryptions.Exceptions
{
    public class DecryptionCoordinationValidationException : Xeption
    {
        public DecryptionCoordinationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
