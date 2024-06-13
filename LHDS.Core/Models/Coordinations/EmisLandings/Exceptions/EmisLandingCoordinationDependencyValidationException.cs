// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace LHDS.Core.Models.Coordinations.EmisLandings.Exceptions
{
    public class EmisLandingCoordinationDependencyValidationException : Xeption
    {
        public EmisLandingCoordinationDependencyValidationException(string message, Xeption? innerException)
            : base(message, innerException)
        { }
    }
}
