// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Foundations.Decryptions.Exceptions
{
    public class DecryptionDependencyException : Xeption
    {
        public DecryptionDependencyException(string message, Xeption innerException) :
            base(message, innerException)
        { }
    }
}