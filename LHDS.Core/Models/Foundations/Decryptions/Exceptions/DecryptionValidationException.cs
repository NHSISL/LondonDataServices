// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionValidationException : Xeption
    {
        public DecryptionValidationException(Xeption innerException)
            : base(message: "Decryption validation errors occurred, please try again.",
                  innerException)
        { }
    }
}