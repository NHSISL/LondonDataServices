// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.DecryptClient
{
    public class DecryptionClientDependencyException : Xeption
    {
        public DecryptionClientDependencyException(Xeption innerException)
            : base(message: "Decryption client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
