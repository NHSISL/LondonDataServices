// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Landings.Client.Models.Clients.LandingClient
{
    public class LandingClientDependencyException : Xeption
    {
        public LandingClientDependencyException(Xeption innerException)
            : base(message: "Landing client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
