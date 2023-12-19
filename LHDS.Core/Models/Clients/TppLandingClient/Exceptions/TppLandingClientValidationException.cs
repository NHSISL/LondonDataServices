// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.TppLandingClient.Exceptions
{
    public class TppLandingClientValidationException : Xeption
    {
        public TppLandingClientValidationException(Xeption innerException)
            : base(
                message: "TPP Landing client validation error occurred, fix errors and try again.",
                innerException)
        { }
    }
}
