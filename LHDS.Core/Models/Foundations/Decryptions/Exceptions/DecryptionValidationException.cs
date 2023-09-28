// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionValidationException : Xeption
    {
        public DecryptionValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}