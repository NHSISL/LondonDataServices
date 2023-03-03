// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LandingClient.Exceptions
{
    public class LandingClientServiceException : Xeption
    {
        public LandingClientServiceException(Xeption innerException)
            : base(message: "Landing client service error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
