// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Clients.LandingClient
{
    public class LandingClientServiceException : Xeption
    {
        public LandingClientServiceException(Xeption innerException)
            : base(message: "Standardly client service error occurred, fix errors and try again.",
                  innerException)
        { }
    }
}
