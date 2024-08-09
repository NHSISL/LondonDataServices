// ---------------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.TppLandings.Exceptions
{
    public class TppLandingCoordinationValidationException : Xeption
    {
        public TppLandingCoordinationValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
