// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.PdsClient.Exceptions
{
    public class PdsClientServiceException : Xeption
    {
        public PdsClientServiceException(Xeption innerException)
            : base(message: "PDS client service error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
