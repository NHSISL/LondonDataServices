// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Clients.LandingClient.Exceptions
{
    public class LandingClientServiceException : Xeption
    {
        public LandingClientServiceException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
