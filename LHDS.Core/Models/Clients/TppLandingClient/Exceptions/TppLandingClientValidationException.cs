// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.TppLandingClient.Exceptions
{
    public class TppLandingClientValidationException : Xeption
    {
        public TppLandingClientValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
