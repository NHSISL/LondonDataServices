// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.EmisLandings.Exceptions
{
    public class EmisLandingCoordinationDependencyException : Xeption
    {
        public EmisLandingCoordinationDependencyException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
