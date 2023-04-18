// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.DecryptClient.Exceptions
{
    public class DecryptionClientValidationException : Xeption
    {
        public DecryptionClientValidationException(Xeption innerException)
            : base(
                message: "Decryption client validation error occurred, fix errors and try again.",
                innerException)
        { }
    }
}
