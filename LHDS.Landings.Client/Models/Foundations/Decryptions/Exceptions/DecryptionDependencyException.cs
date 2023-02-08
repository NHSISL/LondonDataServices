// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionDependencyException : Xeption
    {
        public DecryptionDependencyException(Xeption innerException) :
            base(message: "Decryption dependency error occurred, contact support.", innerException)
        { }
    }
}