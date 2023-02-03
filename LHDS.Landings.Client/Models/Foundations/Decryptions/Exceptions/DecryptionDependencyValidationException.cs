// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Downloads.Exceptions
{
    public class DecryptionDependencyValidationException : Xeption
    {
        public DecryptionDependencyValidationException(Xeption innerException)
            : base(message: "Decryption dependency validation occurred, please try again.", innerException)
        { }
    }
}