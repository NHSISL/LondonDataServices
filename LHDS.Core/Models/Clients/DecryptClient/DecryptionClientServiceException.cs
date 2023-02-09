// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.DecryptionClient
{
    public class DecryptionClientServiceException : Xeption
    {
        public DecryptionClientServiceException(Xeption innerException)
            : base(message: "Standardly client service error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
