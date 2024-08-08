// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.TppLandings.Exceptions
{
    public class InvalidArgumentTppLandingCoordinationException : Xeption
    {
        public InvalidArgumentTppLandingCoordinationException(string message)
            : base(message)
        { }
    }
}