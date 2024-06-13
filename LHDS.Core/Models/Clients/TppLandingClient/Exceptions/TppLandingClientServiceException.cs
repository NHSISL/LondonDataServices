// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.TppLandingClient.Exceptions
{
    public class TppLandingClientServiceException : Xeption
    {
        public TppLandingClientServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
