// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LandingClient.Exceptions
{
    public class LandingClientDependencyException : Xeption
    {
        public LandingClientDependencyException(Xeption innerException)
            : base(message: "Landing client dependency error occurred, contact support.",
                  innerException)
        { }
    }
}
