// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionDependencyValidationException : Xeption
    {
        public DecryptionDependencyValidationException(Xeption innerException)
            : base(message: "Decryption dependency validation occurred, please try again.", innerException) { }
    }
}