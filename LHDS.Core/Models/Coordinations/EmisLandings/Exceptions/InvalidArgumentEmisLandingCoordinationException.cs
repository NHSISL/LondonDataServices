// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.EmisLandings.Exceptions
{
    public class InvalidArgumentEmisLandingCoordinationException : Xeption
    {
        public InvalidArgumentEmisLandingCoordinationException(string message)
            : base(message)
        { }
    }
}