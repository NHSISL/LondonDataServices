// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LandingClient.Exceptions
{
    public class LandingClientValidationException : Xeption
    {
        public LandingClientValidationException(Xeption innerException)
            : base(
                message: "Landing client validation error occurred, fix errors and try again.",
                innerException)
        { }
    }
}
